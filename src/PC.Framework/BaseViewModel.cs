using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using ReactiveUI;
using Shiny;
using Shiny.Net;
using Shiny.Stores;

namespace PC.Framework;

public abstract class BaseViewModel(BaseServices services) : ReactiveObject, IDestructible, IValidationViewModel
{
    protected BaseServices Services { get; } = services;
    protected INavigationService Navigation => Services.Navigation;
    private ICommand? _navigateCommand;
    public ICommand Navigate => _navigateCommand ??= Navigation.GeneralNavigateCommand();


    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }


    private string? _title;
    public string? Title
    {
        get => _title;
        protected set => this.RaiseAndSetIfChanged(ref _title, value);
    }


    private bool? _internetAvailable;
    public virtual bool IsInternetAvailable
    {
        get
        {
            if (_internetAvailable != null) return _internetAvailable!.Value;
            _internetAvailable = Services.Connectivity.IsInternetAvailable();

            Services
                .Connectivity
                .WhenInternetStatusChanged()
                .SubOnMainThread(x =>
                {
                    _internetAvailable = x;
                    this.RaisePropertyChanged();
                })
                .DisposeWith(DestroyWith);
            return _internetAvailable!.Value;
        }
    }


    protected virtual void BindValidation()
    {        
        if (Validation == null && Services.Validation != null)
        {
            Validation = Services.Validation.Bind(this);
            DestroyWith.Add(Validation);
        }
    }



    public virtual IValidationBinding? Validation { get; private set; }


    private CompositeDisposable? _deactivateWith;
    protected internal CompositeDisposable DeactivateWith => _deactivateWith ??= new CompositeDisposable();


    private CompositeDisposable? _destroyWith;
    protected internal CompositeDisposable DestroyWith => _destroyWith ??= new CompositeDisposable();


    private CancellationTokenSource? _deactiveToken;
    /// <summary>
    /// The destroy cancellation token - called when your model is deactivated
    /// </summary>
    protected CancellationToken DeactivateToken
    {
        get
        {
            _deactiveToken ??= new CancellationTokenSource();
            return _deactiveToken.Token;
        }
    }


    private CancellationTokenSource? _destroyToken;
    /// <summary>
    /// The destroy cancellation token - called when your model is destroyed
    /// </summary>
    protected CancellationToken DestroyToken
    {
        get
        {
            _destroyToken ??= new CancellationTokenSource();
            return _destroyToken.Token;
        }
    }

    private ILogger? _logger;
    /// <summary>
    /// A lazy loader logger instance for this viewmodel instance
    /// </summary>
    protected ILogger Logger => _logger ??= Services.LoggerFactory.CreateLogger(GetType());

#if PLATFORM
    /// <summary>
    /// Access to platform services
    /// </summary>
    public IPlatform Platform => Services.Platform;
#endif

    /// <summary>
    /// Dialog service from the service provider
    /// </summary>
    public IDialogs Dialogs => Services.Dialogs;

    private IStringLocalizer? _localizer;
    /// <summary>
    /// The localization source for this instance - will attempt to use the default section (if registered)
    /// </summary>
    public IStringLocalizer? Localize => _localizer ??= Services.StringLocalizationFactory!.Create(GetType());


    /// <summary>
    /// Shiny Connectivity Service 
    /// </summary>
    public IConnectivity Connectivity => Services.Connectivity;

    /// <summary>
    /// Store binder
    /// </summary>
    public IObjectStoreBinder StoreBinder => Services.ObjectBinder;

    /// <summary>
    /// Monitors for viewmodel changes and returns true if valid - handy for ReactiveCommand in place of WhenAny
    /// </summary>
    /// <returns></returns>
    public IObservable<bool> WhenValid()
    {
        if (Services.Validation == null)
            throw new InvalidOperationException("Validation service is not registered");

        return this.WhenAnyProperty().Select(_ => Services.Validation.IsValid(this));
    }


    /// <summary>
    /// Will trap any errors - log them and display a message to the user
    /// </summary>
    /// <param name="action"></param>
    protected virtual async void SafeExecute(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception ex)
        {
            await Services.ErrorHandler.Process(ex);
        }
    }


    /// <summary>
    /// Will trap any errors - log them and display a message to the user
    /// </summary>
    /// <param name="func"></param>
    /// <param name="markBusy"></param>
    /// <returns></returns>
    protected virtual async Task SafeExecuteAsync(Func<Task> func, bool markBusy = false)
    {
        try
        {
            if (markBusy)
                IsBusy = true;

            await func.Invoke().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await Services.ErrorHandler.Process(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }


    /// <summary>
    /// This can be called manually, generally used when your viewmodel is going to the background in the nav stack
    /// </summary>
    protected virtual void Deactivate()
    {
        _deactiveToken?.Cancel();
        _deactiveToken?.Dispose();
        _deactiveToken = null;

        _deactivateWith?.Dispose();
        _deactivateWith = null;
    }


    /// <summary>
    /// Called when the viewmodel is being destroyed (not in nav stack any longer)
    /// </summary>
    public virtual void Destroy()
    {
        _destroyToken?.Cancel();
        _destroyToken?.Dispose();

        Deactivate();
        _destroyWith?.Dispose();
    }


    /// <summary>
    /// Binds to IsBusy while your command works
    /// </summary>
    /// <param name="command"></param>
    protected void BindBusyCommand(ICommand command)
        => BindBusyCommand((IReactiveCommand)command);


    /// <summary>
    /// Binds to IsBusy while your command works
    /// </summary>
    /// <param name="command"></param>
    protected void BindBusyCommand(IReactiveCommand command) =>
        command.IsExecuting.Subscribe(
            x => IsBusy = x,
            _ => IsBusy = false,
            () => IsBusy = false
        )
        .DisposeWith(DeactivateWith);


    /// <summary>
    /// Records the state of this model type for all get/set properties
    /// </summary>
    protected virtual void RememberUserState()
    {
        Services.ObjectBinder.Bind(this);
        DestroyWith.Add(Disposable.Create(() =>
            Services.ObjectBinder.UnBind(this)
        ));
    }


    /// <summary>
    /// Reads localization key from localization service
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual string this[string key]
    {
        get
        {
            if (Localize == null)
                throw new InvalidOperationException("Localization has not been initialized in your DI container");

            var res = Localize[key];
            return res.ResourceNotFound ? $"KEY '{key}' NOT FOUND" : res.Value;
        }
    }
}
