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
        private readonly List<ICoreWpfPlugin> CoreWpfPlugins = new List<ICoreWpfPlugin>();
        private TaskCompletionSource<bool> WaitComplete = new TaskCompletionSource<bool>();

        public EmberWpfUIService()
        {
        }

        internal void RegisterWpfPlugin(ICoreWpfPlugin wpfPlugin)
        {
            this.CoreWpfPlugins.Add(wpfPlugin);
        }

        internal async Task StartWpfUIService()
        {
            UIThread = new Thread(() =>
            {
                Application = new Application
                {
                    ShutdownMode = ShutdownMode.OnExplicitShutdown
                };
                foreach (var configure in CoreWpfPlugins)
                {
                    configure.BuildApplication(Application);
                }
                Application.Startup += (_, __) => { WaitComplete.SetResult(true); };
                Application.Run();
            });
            UIThread.SetApartmentState(ApartmentState.STA);
            UIThread.Start();
            await WaitComplete.Task;
        }

        public void Dispose()
        {
            Application.Dispatcher.DisableProcessing();
            Application.Dispatcher.InvokeShutdown();
            UIThread?.Abort();
        }

        private async ValueTask RegisterWindow<TWindow>(CancellationToken token = default)
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

        private async ValueTask BeginIHostedWindowScope<TWindow>(Action<TWindow> scope)
            where TWindow : Window, IHostedWindow, new()
        {
            var app = (TWindow)RunningWindows[typeof(TWindow)];
            await Application.Dispatcher.InvokeAsync(() => scope(app));
        }

        public void Register<TWindow>()
            where TWindow : Window, IHostedWindow, new()
        {
            RegisterWindow<TWindow>().AsTask().Wait();
        }

        public async ValueTask RegisterAsync<TWindow>(CancellationToken token = default)
            where TWindow : Window, IHostedWindow, new()
        {
            await RegisterWindow<TWindow>(token);
        }

        public async ValueTask InitializeAsync<TWindow>(ILifetimeScope initializeScope)
            where TWindow : Window, IHostedWindow, new()
        {
            await BeginIHostedWindowScope<TWindow>((app) => app.Initialize(initializeScope));
        }

        public async ValueTask UninitializeAsync<TWindow>(ILifetimeScope initializeScope)
            where TWindow : Window, IHostedWindow, new()
        {
            await BeginIHostedWindowScope<TWindow>((app) => app.Uninitialize(initializeScope));
            RunningWindows.Remove(typeof(TWindow));
        }

        public async ValueTask BeginUIThreadScope(Func<ValueTask> scope)
        {
            await Application.Dispatcher.InvokeAsync(() => scope());
        }

        public async ValueTask<TResult> BeginUIThreadScope<TResult>(Func<TResult> scope)
        {
            return await Application.Dispatcher.InvokeAsync(() => scope());
        }

        public async ValueTask BeginUIThreadScope(Action scope)
        {
            await Application.Dispatcher.InvokeAsync(() => scope());
        }
    }
}
