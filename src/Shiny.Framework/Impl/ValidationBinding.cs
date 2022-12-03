﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Shiny.Impl;


public class ValidationBinding : ReactiveObject, IValidationBinding
{
    readonly IDisposable dispose;
    readonly Dictionary<string, bool> touched = new();
    readonly Dictionary<string, string> errors = new();


    public ValidationBinding(IValidationService service, IReactiveObject reactiveObj)
    {
        this.dispose = reactiveObj
            .WhenAnyProperty()
            .SubOnMainThread(x =>
            {
                if (x.PropertyName == null)
                {
                    var results = service.Validate(reactiveObj);
                    this.errors.Clear();

                    foreach (var result in results)
                    {
                        var msg = result.Value?.FirstOrDefault();
                        this.Set(result.Key, msg);
                    }
                }
                else
                {
                    var error = service.ValidateProperty(reactiveObj, x.PropertyName)?.FirstOrDefault();
                    this.Set(x.PropertyName, error);
                }
            });
    }


    public IReadOnlyDictionary<string, string> Errors => this.errors;
    public IReadOnlyDictionary<string, bool> Touched => this.touched;


    internal void Set(string propertyName, string? errorMessage)
    {
        if (!this.touched.ContainsKey(propertyName))
        {
            this.touched[propertyName] = true;
            this.RaisePropertyChanged(nameof(this.Touched));
        }
        if (this.errors.ContainsKey(propertyName))
        {
            // change
            this.errors.Remove(propertyName);
            if (errorMessage != null)
                this.errors[propertyName] = errorMessage;

            this.RaisePropertyChanged(nameof(this.Errors));
        }
        else if (errorMessage != null)
        {
            // change
            this.errors[propertyName] = errorMessage;
            this.RaisePropertyChanged(nameof(this.Errors));
        }
    }


    public void Dispose() => this.dispose.Dispose();
}
