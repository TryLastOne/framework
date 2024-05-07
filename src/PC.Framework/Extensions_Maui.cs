#if PLATFORM
using System;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Maui;
using PC.Framework.Impl;
using Prism.Ioc;
using Prism;
using ReactiveUI;
using Shiny;

namespace PC.Framework;


public static partial class MauiExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="container"></param>
    /// <param name="prismBuilder"></param>
    /// <param name="exceptionConfig"></param>
    /// <returns></returns>
    public static MauiAppBuilder UsePcFramework(this MauiAppBuilder builder, IContainerExtension container, Action<PrismAppBuilder> prismBuilder, GlobalExceptionHandlerConfig? exceptionConfig = null)
    {
        builder
            .UsePrism(container, prismBuilder)
            .UseShiny();
      
        builder.Services.AddSingleton<IGlobalNavigationService, GlobalNavigationService>();

        builder.Services.TryAddSingleton(AppInfo.Current);
        builder.Services.TryAddSingleton(exceptionConfig ?? new GlobalExceptionHandlerConfig());
        builder.Services.TryAddSingleton<GlobalExceptionAction>();
        builder.Services.AddConnectivity();
        builder.Services.AddScoped<BaseServices>();

        RxApp.DefaultExceptionHandler = new GlobalExceptionHandler();
        return builder;
    }
}
#endif
