﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace PC.Framework.Impl;


/// <summary>
/// 
/// </summary>
/// <param name="localizationManager"></param>
public class DataAnnotationsValidationService(IStringLocalizerFactory? localizationManager = null) : IValidationService
{
    public bool IsValid(object obj, string? propertyName = null)
    {
        if (propertyName != null) return !ValidateProperty(obj, propertyName).Any();
        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();
        var result = Validator.TryValidateObject(
            obj,
            context,
            results,
            true
        );
        return result;
    }


    public IValidationBinding Bind(IValidationViewModel viewModel)
        => new ValidationBinding(this, viewModel);


    public IDictionary<string, IList<string>> Validate(object obj)
    {
        var values = new Dictionary<string, IList<string>>();
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(
            obj,
            new ValidationContext(obj),
            results
        );

        foreach (var result in results)
        {
            foreach (var member in result.MemberNames)
            {
                if (!values.ContainsKey(member))
                    values.Add(member, new List<string>());

                var errMsg = GetErrorMessage(obj, result);
                values[member].Add(errMsg);
            }
        }
        return values;
    }


    public IEnumerable<string> ValidateProperty(object obj, string propertyName)
    {
        var results = new List<ValidationResult>();
        var value = GetValue(obj, propertyName);

        Validator.TryValidateProperty(
            value,
            new ValidationContext(obj) { MemberName = propertyName },
            results
        );
        foreach (var result in results)
        {
            if (result.MemberNames.Contains(propertyName))
            {
                yield return GetErrorMessage(obj, result);
            }
        }
    }


    protected virtual string GetErrorMessage(object obj, ValidationResult result)
    {
        if (!(result.ErrorMessage?.StartsWith("localize:") ?? false)) return result.ErrorMessage!;
        if (localizationManager == null)
            throw new ArgumentException("Localization has not been put into your startup");

        var key = result.ErrorMessage.Replace("localize:", String.Empty);
        var localize = localizationManager.Create(obj.GetType());

        return localize[key];
    }


    protected static object? GetValue(object obj, string propertyName)
        => obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
}