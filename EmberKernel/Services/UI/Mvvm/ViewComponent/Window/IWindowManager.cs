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
        Task RegisterAsync<TWindow>(CancellationToken token = default) where TWindow : T, IHostedWindow, new();
        Task InitializeAsync<TWindow>(ILifetimeScope initializeScope) where TWindow : T, IHostedWindow, new();
        Task UninitializeAsync<TWindow>(ILifetimeScope initializeScope) where TWindow : T, IHostedWindow, new();
    }

    public interface IWindowManager
    {
        Task BeginUIThreadScope(Action scope);
        Task BeginUIThreadScope(Func<Task> scope);
        Task<TResult> BeginUIThreadScope<TResult>(Func<TResult> scope);
    }
}
