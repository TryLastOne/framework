using System.Threading.Tasks;

namespace PC.Framework;

/// <summary>
/// 
/// </summary>
public interface IDialogs
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="dismissText"></param>
    /// <returns></returns>
    Task Alert(string message, string? title = null, string? dismissText = "OK");
    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="acceptText"></param>
    /// <param name="dismissText"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<string?> ActionSheet(string title, string? acceptText = null, string? dismissText = null, params string[] options);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="title"></param>
    /// <param name="acceptText"></param>
    /// <param name="dismissText"></param>
    /// <returns></returns>
    Task<bool> Confirm(string message, string? title = null, string? acceptText = "OK", string? dismissText = "Cancel");
    /// <summary>
    /// 
    /// </summary>
    /// <param name="question"></param>
    /// <param name="title"></param>
    /// <param name="acceptText"></param>
    /// <param name="dismissText"></param>
    /// <param name="placeholder"></param>
    /// <param name="maxLength"></param>
    /// <param name="keyboard"></param>
    /// <returns></returns>
    Task<string?> Input(string question, string? title = null, string? acceptText = "OK", string? dismissText = "Cancel", string? placeholder = null, int? maxLength = null, InputKeyboard keyboard = InputKeyboard.Default);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="durationMillis"></param>
    /// <param name="actionText"></param>
    /// <returns></returns>
    Task<bool> Snackbar(string message, int durationMillis = 3000, string? actionText = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="durationMillis"></param>
    /// <param name="fontSize"></param>
    /// <returns></returns>
    Task Toast(string message, int durationMillis = 4000, double fontSize = 14.0);
}


/// <summary>
/// 
/// </summary>
public enum InputKeyboard
{
    /// <summary>
    /// 
    /// </summary>
    Chat,
    /// <summary>
    /// 
    /// </summary>
    Default,
    /// <summary>
    /// 
    /// </summary>
    Email,
    /// <summary>
    /// 
    /// </summary>
    Numeric,
    /// <summary>
    /// 
    /// </summary>
    Plain,
    /// <summary>
    /// 
    /// </summary>
    Telephone,
    /// <summary>
    /// 
    /// </summary>
    Text,
    /// <summary>
    /// 
    /// </summary>
    Url
}