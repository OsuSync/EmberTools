using Autofac;
using EmberKernel;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace EmberCore.KernelServices.UI.View
{
    public class EmberWpfUIService : IWindowManager<Window>, IKernelService, IDisposable
    {
        public Application Application { get; private set; }
        private Thread UIThread { get; set; }
        private Dictionary<Type, Window> RunningWindows { get; } = new Dictionary<Type, Window>();
        private SemaphoreSlim InitSemaphore { get; } = new SemaphoreSlim(1);

        public EmberWpfUIService()
        {
            TaskCompletionSource<bool> waitComplete = new TaskCompletionSource<bool>();
            UIThread = new Thread(() =>
            {
                Application = new Application
                {
                    ShutdownMode = ShutdownMode.OnExplicitShutdown
                };
                waitComplete.SetResult(true);
                Application.Run();
            });
            UIThread.SetApartmentState(ApartmentState.STA);
            UIThread.Start();
            waitComplete.Task.Wait();
        }

        public void Dispose()
        {
            Application.Dispatcher.DisableProcessing();
            Application.Dispatcher.InvokeShutdown();
            UIThread?.Abort();
        }

        private async Task RegisterWindow<TWindow>(CancellationToken token = default)
            where TWindow : Window, IHostedWindow, new()
        {
            await InitSemaphore.WaitAsync(token);
            try
            {
                if (!RunningWindows.ContainsKey(typeof(TWindow)))
                {
                    var window = await Application.Dispatcher.InvokeAsync(() => new TWindow());
                    RunningWindows.Add(typeof(TWindow), window);
                }
            }
            finally
            {
                InitSemaphore.Release();
            }
        }

        private async Task BeginIHostedWindowScope<TWindow>(Action<TWindow> scope)
            where TWindow : Window, IHostedWindow, new()
        {
            var app = (TWindow)RunningWindows[typeof(TWindow)];
            await Application.Dispatcher.InvokeAsync(() => scope(app));
        }

        public void Register<TWindow>()
            where TWindow : Window, IHostedWindow, new()
        {
            RegisterWindow<TWindow>().Wait();
        }

        public async Task RegisterAsync<TWindow>(CancellationToken token = default)
            where TWindow : Window, IHostedWindow, new()
        {
            await RegisterWindow<TWindow>(token);
        }

        public async Task InitializeAsync<TWindow>(ILifetimeScope initializeScope)
            where TWindow : Window, IHostedWindow, new()
        {
            await BeginIHostedWindowScope<TWindow>((app) => app.Initialize(initializeScope));
        }

        public async Task UninitializeAsync<TWindow>(ILifetimeScope initializeScope)
            where TWindow : Window, IHostedWindow, new()
        {
            await BeginIHostedWindowScope<TWindow>((app) => app.Uninitialize(initializeScope));
            RunningWindows.Remove(typeof(TWindow));
        }

        public async Task BeginUIThreadScope(Func<Task> scope)
        {
            await Application.Dispatcher.InvokeAsync(() => scope());
        }

        public async Task<TResult> BeginUIThreadScope<TResult>(Func<TResult> scope)
        {
            return await Application.Dispatcher.InvokeAsync(() => scope());
        }

        public async Task BeginUIThreadScope(Action scope)
        {
            await Application.Dispatcher.InvokeAsync(() => scope());
        }
    }
}
