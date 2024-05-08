using System;
using System.Threading.Tasks;
using Prism.Navigation;

namespace PC.Framework;


public abstract class FuncViewModel(BaseServices services) : ViewModel(services)
{
    protected Action? Appearing { get; set; }
    protected Func<Task>? AppearingTask { get; set; }
    protected Action? BeforeDestroy { get; set; }
    protected Action? Disappearing { get; set; }
    protected Func<INavigationParameters, Task>? Init { get; set; }
    protected Func<Task<bool>>? CanNav { get; set; }
    protected Action<INavigationParameters>? NavTo { get; set; }
    protected Func<INavigationParameters, Task>? NavToTask { get; set; }
    protected Action<INavigationParameters>? NavFrom { get; set; }
    protected Func<IDisposable[]>? WithDisappear { get; set; }
    protected Func<IDisposable[]>? WithDestroy { get; set; }

    public override Task InitializeAsync(INavigationParameters parameters)
    {
        if (Init == null)
            return Task.CompletedTask;

        if (WithDestroy == null) return Init.Invoke(parameters);
        var en = WithDestroy.Invoke();
        DestroyWith.AddRange(en);

        return Init.Invoke(parameters);
    }


    public override Task<bool> CanNavigateAsync(INavigationParameters parameters)
    {
        return CanNav == null ? Task.FromResult(true) : CanNav.Invoke();
    }


    public override async void OnNavigatedTo(INavigationParameters parameters)
    {
        base.OnNavigatedTo(parameters);
        NavTo?.Invoke(parameters);

        if (NavToTask != null)
            await SafeExecuteAsync(() => NavToTask.Invoke(parameters));
    }


    public override void OnNavigatedFrom(INavigationParameters parameters)
    {
        base.OnNavigatedFrom(parameters);
        NavFrom?.Invoke(parameters);
    }


    public override async void OnAppearing()
    {
        base.OnAppearing();
        Appearing?.Invoke();
        if (AppearingTask != null)
            await SafeExecuteAsync(AppearingTask);

        if (WithDisappear == null) return;
        var en = WithDisappear.Invoke();
        DeactivateWith.AddRange(en);
    }


    public override void OnDisappearing()
    {
        base.OnDisappearing();
        Disappearing?.Invoke();
    }


    public override void Destroy()
    {
        base.Destroy();
        BeforeDestroy?.Invoke();
    }
}