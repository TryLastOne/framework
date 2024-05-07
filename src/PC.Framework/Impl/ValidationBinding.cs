using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Shiny;

namespace PC.Framework.Impl;

public class ValidationBinding : ReactiveObject, IValidationBinding
{
    private readonly IDisposable _dispose;
    private readonly Dictionary<string, bool> _touched = new();
    private readonly Dictionary<string, string> _errors = new();
    private readonly Dictionary<string, bool> _isErrors = new();

    public ValidationBinding(IValidationService service, IReactiveObject reactiveObj)
    {
        _dispose = reactiveObj
            .WhenAnyProperty()
            .SubOnMainThread(x =>
            {
                if (x.PropertyName == null)
                {
                    var results = service.Validate(reactiveObj);
                    _errors.Clear();

                    foreach (var result in results)
                    {
                        var msg = result.Value.FirstOrDefault();
                        Set(result.Key, msg);
                    }
                }
                else
                {
                    var error = service.ValidateProperty(reactiveObj, x.PropertyName).FirstOrDefault();
                    Set(x.PropertyName, error);
                }
            });
    }


    public IReadOnlyDictionary<string, string> Errors => _errors;
    public IReadOnlyDictionary<string, bool> Touched => _touched;
    public IReadOnlyDictionary<string, bool> IsError => _isErrors;

    internal void Set(string propertyName, string? errorMessage)
    {
        if (_touched.TryAdd(propertyName, true))
        {
            _isErrors.Add(propertyName, false);
            this.RaisePropertyChanged(nameof(Touched));
        }
        if (_errors.ContainsKey(propertyName))
        {
            // change
            _errors.Remove(propertyName);
            _isErrors[propertyName] = true;
            if (errorMessage != null)
                _errors[propertyName] = errorMessage;
        }
        else if (errorMessage != null)
        {
            // change
            _errors[propertyName] = errorMessage;
            _isErrors[propertyName] = false;
        }
        this.RaisePropertyChanged(nameof(IsError));
        this.RaisePropertyChanged(nameof(Errors));
    }


    public void Dispose() => _dispose.Dispose();
}
