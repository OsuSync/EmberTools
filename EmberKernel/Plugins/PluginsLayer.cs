using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace EmberKernel.Plugins
{
    public class PluginsLayer<T> : IPluginsLayer where T : IPluginsLoader
    {
        public void BuildScope(ContainerBuilder builder)
        {
            builder.RegisterType<T>();
        }

        public async Task Run(ILifetimeScope scope)
        {
            var pluginLoader = scope.Resolve<T>();
            using var pluginsScope = scope.BeginLifetimeScope(builder =>
            {
                pluginLoader.BuildScope(builder);
            });

            await pluginLoader.Run(pluginsScope);
            await pluginLoader.RunEntryComponents();
        }
    }
}
