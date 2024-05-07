using System;
using System.Collections.Generic;

namespace PC.Framework;


/// <summary>
/// 
/// </summary>
public interface IValidationBinding : ReactiveUI.IReactiveObject, IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    IReadOnlyDictionary<string, bool> IsError { get; }
    /// <summary>
    /// 
    /// </summary>
    IReadOnlyDictionary<string, string> Errors { get; }
    /// <summary>
    /// 
    /// </summary>
    IReadOnlyDictionary<string, bool> Touched { get;}
}
