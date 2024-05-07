namespace PC.Framework;

/// <summary>
/// 
/// </summary>
public enum ErrorAlertType
{
    /// <summary>
    /// 
    /// </summary>
    None,
    /// <summary>
    /// 
    /// </summary>
    NoLocalize,
    /// <summary>
    /// 
    /// </summary>
    FullError,
    /// <summary>
    /// 
    /// </summary>
    Localize
}


/// <summary>
/// 
/// </summary>
/// <param name="AlertType"></param>
/// <param name="LocalizeErrorTitleKey"></param>
/// <param name="LocalizeErrorBodyKey"></param>
/// <param name="IgnoreTokenCancellations"></param>
/// <param name="LogError"></param>
public record GlobalExceptionHandlerConfig(
    ErrorAlertType AlertType = ErrorAlertType.NoLocalize,
    string? LocalizeErrorTitleKey = null,
    string? LocalizeErrorBodyKey = null,
    bool IgnoreTokenCancellations = true,
    bool LogError = true
);