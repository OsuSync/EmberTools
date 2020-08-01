﻿using Autofac;
using EmberKernel.Plugins;
using System;
using System.Threading.Tasks;

namespace EmberKernel
{
    public class Kernel : IDisposable
    {
        private ContainerBuilder Builder { get; }
        private IContainer Container { get; }
        private ILifetimeScope executionScope = null;

        public Kernel(ContainerBuilder builder)
        {
            Builder = builder;
            Builder.RegisterInstance(this).AsSelf().SingleInstance();
            Container = Builder.Build();
        }

        public void Run()
        {
            RunAsync().AsTask().Wait();
        }

        public async ValueTask Exit()
        {
            if (Container.IsRegistered<IPluginsLayer>())
            {
                await Container.Resolve<IPluginsLayer>().DisposeAsync();
            }
            await Container.DisposeAsync();
        }

        public async ValueTask RunAsync()
        {
            var pluginLayer = Container.Resolve<IPluginsLayer>();
            executionScope = Container.BeginLifetimeScope(builder =>
            {
                if (Container.IsRegistered<IPluginsLayer>())
                {
                    pluginLayer.BuildScope(builder);
                }
            });

            await pluginLayer.Run(executionScope);
        }

        public void Dispose()
        {
            executionScope.Dispose();
        }
    }
}
