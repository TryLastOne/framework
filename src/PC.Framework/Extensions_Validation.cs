using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Shiny.Reflection;

namespace PC.Framework;


public static class ValidationExtensions
{
    /// <summary>
    /// Checks an object property and returns true if valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="obj"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static bool IsValidProperty<T>(this IValidationService service, T? obj, Expression<Func<T, string>>? expression)
        => obj != null && expression != null && service.IsValid(obj, obj.GetPropertyInfo(expression)?.Name);


    /// <summary>
    /// Checks an object property to see if it is valid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="service"></param>
    /// <param name="obj"></param>
    /// <param name="expression"></param>
    /// <returns></returns>

    public static IEnumerable<string> ValidateProperty<T>(this IValidationService service, T? obj, Expression<Func<T, string>>? expression)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentNullException.ThrowIfNull(expression);

        var propertyInfo = obj.GetPropertyInfo(expression);

        return propertyInfo is null ? new List<string>() : service.ValidateProperty(obj, propertyInfo.Name);
    }
}

