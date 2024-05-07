namespace PC.Framework;


/// <summary>
/// 
/// </summary>
public interface IValidationViewModel : ReactiveUI.IReactiveObject // needs to be idisposable or something to destroy
{
    /// <summary>
    /// 
    /// </summary>
    IValidationBinding? Validation { get; }
}
