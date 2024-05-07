using System;
using Microsoft.Extensions.DependencyInjection;
using Shiny.Hosting;

namespace PC.Framework.Impl;


/// <summary>
/// 
/// </summary>
public class GlobalExceptionHandler : IObserver<Exception>
{
    /// <summary>
    /// 
    /// </summary>
    public void OnCompleted() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    public void OnError(Exception error) { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    public async void OnNext(Exception value) => await Host
        .Current
        .Services
        .GetRequiredService<GlobalExceptionAction>()
        .Process(value);
}