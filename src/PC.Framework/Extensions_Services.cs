using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PC.Framework.Impl;

namespace PC.Framework;


public static class ServiceExtensions
{
    public static void AddDataAnnotationValidation(this IServiceCollection services)
        => services.TryAddSingleton<IValidationService, DataAnnotationsValidationService>();
}
