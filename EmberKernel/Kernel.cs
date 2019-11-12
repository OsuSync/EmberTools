using Autofac;
using EmberKernel.Loader;
using EmberKernel.Plugins;
using System;
using System.Collections.Generic;

namespace EmberKernel
{
    public class Kernel
    {
        ContainerBuilder Builder { get; }
        IContainer Container { get; }

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
                if (scope.IsRegistered<ILoader>())
                {
                    executionLayerBuilders.Add(scope.Resolve<ILoader>());
                }

                if (scope.IsRegistered<IPluginLoader>())
                {
                    executionLayerBuilders.Add(scope.Resolve<IPluginLoader>());
                }

                if (scope.IsRegistered<IPluginManager>())
                {
                    executionLayerBuilders.Add(scope.Resolve<IPluginManager>());
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
