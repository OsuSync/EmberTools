using Autofac;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public class PluginsLayer<T> : IPluginsLayer where T : IPluginsLoader
    {
        public void BuildScope(ContainerBuilder builder)
        {
            var pluginManBuilder = builder.RegisterType<T>().As<IPluginsLoader>().SingleInstance();
            if (typeof(IPluginsManager).IsAssignableFrom(typeof(T)))
            {
                pluginManBuilder.As<IPluginsManager>().SingleInstance();
                builder.RegisterType<PluginManagerViewModel>().As<IPluginManagerViewModel>().SingleInstance();
            }
        }

        private ILifetimeScope PluginsScope { get; set; }
        public async ValueTask Run(ILifetimeScope scope)
        {
            var pluginLoader = scope.Resolve<IPluginsLoader>();
            PluginsScope = scope.BeginLifetimeScope(builder =>
            {
                pluginLoader.BuildScope(builder);
            });

            await pluginLoader.Run(PluginsScope);
            await pluginLoader.RunEntryComponents();

            if (scope.TryResolve<IEventBus>(out var eventBus)) {
                eventBus.Publish(EmberInitializedEvent.Empty);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (PluginsScope == null) return;
            var pluginsManager = PluginsScope.ResolveOptional<IPluginsManager>();
            if (pluginsManager != null) await pluginsManager.DisposeAsync();
            await PluginsScope.DisposeAsync();
            PluginsScope = null;
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().Wait();
        }
    }
}
