using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace PC.Framework;

/// <summary>
/// 
/// </summary>
public class GlobalExceptionAction
{
    private readonly GlobalExceptionHandlerConfig _config;
    private readonly IStringLocalizer? _localize;
    private readonly IDialogs _dialogs;
    private readonly ILogger _logger;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    /// <param name="dialogs"></param>
    /// <param name="localize"></param>
    public GlobalExceptionAction(
        GlobalExceptionHandlerConfig config,
        ILogger<GlobalExceptionAction> logger,
        IDialogs dialogs,
        IStringLocalizer<GlobalExceptionAction>? localize = null
    )
    {
        _config = config;
        _dialogs = dialogs;
        _logger = logger;
        _localize = localize;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="cfg"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual bool ShouldIgnore(GlobalExceptionHandlerConfig cfg, Exception value)
    {
        return cfg.IgnoreTokenCancellations && value is TaskCanceledException;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="title"></param>
    /// <param name="body"></param>
    /// <exception cref="ArgumentException"></exception>
    public virtual async Task Process(Exception value, string title = "ERROR", string body = "There was an error.")
    {
        if (ShouldIgnore(_config, value))
            return;

        if (_config.LogError)
            _logger.LogError(value, "Error in view");

        if (_config.AlertType != ErrorAlertType.None)
        {
            if (_dialogs == null)
                throw new ArgumentException("No dialogs registered");
            
            await _dialogs.Alert(body, title);
        }
    }
}

