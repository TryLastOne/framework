using System.Threading.Tasks;
using Prism.AppModel;
using Prism.Navigation;

namespace PC.Framework;


/// <summary>
/// 
/// </summary>
public abstract class ViewModel : BaseViewModel,
                                  IInitializeAsync,
                                  IPageLifecycleAware,
                                  IApplicationLifecycleAware,
                                  INavigatedAware,
                                  IConfirmNavigationAsync
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    protected ViewModel(BaseServices services) : base(services) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public virtual Task InitializeAsync(INavigationParameters parameters)
        => Task.CompletedTask;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public virtual Task<bool> CanNavigateAsync(INavigationParameters parameters)
        => Task.FromResult(true);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    public virtual void OnNavigatedFrom(INavigationParameters parameters)
        => Deactivate();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameters"></param>
    public virtual void OnNavigatedTo(INavigationParameters parameters) { }
    /// <summary>
    /// 
    /// </summary>
    public virtual void OnAppearing() { }
    /// <summary>
    /// 
    /// </summary>
    public virtual void OnDisappearing() { }
    /// <summary>
    /// 
    /// </summary>
    public virtual void OnResume() => OnAppearing();
    /// <summary>
    /// 
    /// </summary>
    public virtual void OnSleep() => OnDisappearing();
}
