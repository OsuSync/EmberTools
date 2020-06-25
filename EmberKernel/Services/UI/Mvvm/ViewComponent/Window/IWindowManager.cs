using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent.Window
{
    public interface IWindowManager<T> : IWindowManager
    {
        void Register<TWindow>() where TWindow : T, IHostedWindow, new();
        ValueTask RegisterAsync<TWindow>(CancellationToken token = default) where TWindow : T, IHostedWindow, new();
        ValueTask InitializeAsync<TWindow>(ILifetimeScope initializeScope) where TWindow : T, IHostedWindow, new();
        ValueTask UninitializeAsync<TWindow>(ILifetimeScope initializeScope) where TWindow : T, IHostedWindow, new();
    }

    public interface IWindowManager
    {
        ValueTask BeginUIThreadScope(Action scope);
        ValueTask BeginUIThreadScope(Func<ValueTask> scope);
        ValueTask<TResult> BeginUIThreadScope<TResult>(Func<TResult> scope);
    }
}
