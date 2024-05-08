using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Shiny;
using Shiny.Net;
using Shiny.Stores;

namespace PC.Framework;


/// <summary>
/// 
/// </summary>
/// <param name="Navigation"></param>
/// <param name="Dialogs"></param>
/// <param name="ObjectBinder"></param>
/// <param name="ErrorHandler"></param>
/// <param name="Connectivity"></param>
/// <param name="LoggerFactory"></param>
/// <param name="Validation"></param>
/// <param name="Platform"></param>
 

public record BaseServices(
#if PLATFORM
    IPlatform Platform,
#endif
    INavigationService Navigation,
    IDialogs Dialogs,
    IObjectStoreBinder ObjectBinder,
    GlobalExceptionAction ErrorHandler,
    IConnectivity Connectivity,
    ILoggerFactory LoggerFactory,
    IValidationService? Validation = null,
    IStringLocalizerFactory? StringLocalizationFactory = null
);
