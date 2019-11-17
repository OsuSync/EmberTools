using Autofac;
using EmberKernel.Plugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmberKernel
{
    public class Kernel : IDisposable
    {
        private ContainerBuilder Builder { get; }
        private IContainer Container { get; }
        private ILifetimeScope topScope = null;
        private ILifetimeScope executionScope = null;

        public Kernel(ContainerBuilder builder)
        {
            Builder = builder;
            Container = Builder.Build();
        }

        public void Run()
        {
            RunAsync().Wait();
        }

        public async Task RunAsync()
        {
            topScope = Container.BeginLifetimeScope(builder =>
            {
                // Build base infrastructures here
            });

            var pluginLayer = topScope.Resolve<IPluginsLayer>();
            // Build execution scope
            executionScope = topScope.BeginLifetimeScope(builder =>
            {
                if (topScope.IsRegistered<IPluginsLayer>())
                {
                    pluginLayer.BuildScope(builder);
                }
            });

            await pluginLayer.Run(executionScope);
        }

        public void Dispose()
        {
            executionScope.Dispose();
            topScope.Dispose();
        }
    }
}
