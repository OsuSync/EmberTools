using System;
using System.Collections.Generic;
using System.Text;
using Autofac;

namespace EmberKernel.Plugins
{
    public class PluginLayer<T> : IPluginManager where T : IPluginLoader
    {
        public PluginLayer()
        {
        }

        public void BuildScope(ContainerBuilder builder)
        {
            builder.RegisterType<T>();
        }

        public void Run(ILifetimeScope scope)
        {
            var pluginLoader = scope.Resolve<T>();
            using (var pluginsScope = scope.BeginLifetimeScope(builder =>
            {
                pluginLoader.BuildScope(builder);
            }))
            {

            }
        }
    }
}
