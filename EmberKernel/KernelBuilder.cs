using Autofac;
using EmberKernel.Loader;
using EmberKernel.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public class KernelBuilder
    {
        internal ContainerBuilder _containerBuilder;
        internal Func<ILoader> _loaderFactory = () => null;
        public KernelBuilder()
        {
            _containerBuilder = new ContainerBuilder();
        }

        public KernelBuilder UseLoader<T>() where T : ILoader, new()
        {
            _containerBuilder.RegisterType<T>().As<ILoader>();
            return this;
        }

        public KernelBuilder UsePlugins<TPluginLoader>() where TPluginLoader : IPluginLoader
        {
            _containerBuilder.RegisterType<PluginLayer<TPluginLoader>>().As<IPluginManager>();
            return this;
        }

        public Kernel Build()
        {
            return new Kernel(_containerBuilder);
        }
    }
}
