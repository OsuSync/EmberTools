using Autofac;
using EmberKernel.Plugins;
using System;
using System.Collections.Generic;

namespace EmberKernel
{
    public class Kernel
    {
        private ContainerBuilder Builder { get; }
        private IContainer Container { get; }

        public Kernel(ContainerBuilder builder)
        {
            Builder = builder;
            Container = Builder.Build();
        }

        public void Run()
        {
            using (var scope = Container.BeginLifetimeScope(builder =>
            {
                // Build base infrastructures here
            }))
            {
                List<IScopeBilder> executionLayerBuilders = new List<IScopeBilder>();

                if (scope.IsRegistered<IPluginsLoader>())
                {
                    executionLayerBuilders.Add(scope.Resolve<IPluginsLoader>());
                }

                if (scope.IsRegistered<IPluginsLayer>())
                {
                    executionLayerBuilders.Add(scope.Resolve<IPluginsLayer>());
                }

                // Build execution scope
                using (var executionScope = scope.BeginLifetimeScope(builder =>
                {
                    foreach (var subBuilder in executionLayerBuilders)
                    {
                        subBuilder.BuildScope(builder);
                    }
                }))
                {
                    foreach (var subBuilder in executionLayerBuilders)
                    {
                        subBuilder.Run(executionScope);
                    }
                }
            }
        }
    }
}
