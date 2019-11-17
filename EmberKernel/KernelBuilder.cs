using Autofac;
using EmberKernel.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using EmberKernel.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using System.IO;

namespace EmberKernel
{
    public class KernelBuilder
    {
        public class KernelBuilderContext
        {
            public IConfigurationRoot Configuration { get; set; }
        }

        internal class ServiceCollectionBuilder : ILoggingBuilder
        {
            public IServiceCollection Services { get; }
            public ServiceCollectionBuilder(IServiceCollection services)
            {
                Services = services;
            }
        }

        internal ContainerBuilder _containerBuilder;
        internal IConfigurationBuilder _configurationBuilder;
        internal ServiceCollectionBuilder _serviceCollectionBuilder;
        internal KernelBuilderContext _context;
        internal bool _hasRegisterContentRoot = false;
        internal bool _useLogger = false;
        internal readonly List<Action> buildActions = new List<Action>();
        public KernelBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            _configurationBuilder = new ConfigurationBuilder();
            _context = new KernelBuilderContext();
            _serviceCollectionBuilder = new ServiceCollectionBuilder(new ServiceCollection());
        }

        public KernelBuilder UsePlugins<TPluginsLoader>() where TPluginsLoader : IPluginsLoader
        {
            buildActions.Add(() => _containerBuilder.RegisterType<PluginsLayer<TPluginsLoader>>().As<IPluginsLayer>());
            return this;
        }

        public KernelBuilder UseKernalService<TKernelService>() where TKernelService : KernelService
        {
            buildActions.Add(() => _containerBuilder.RegisterType<TKernelService>());
            return this;
        }

        public KernelBuilder UseConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(_configurationBuilder);
            _context.Configuration = _configurationBuilder.Build();
            buildActions.Add(() =>
            {
                _containerBuilder.RegisterInstance(_context.Configuration).As<IConfiguration>();
            });
            return this;
        }

        public KernelBuilder UseConfigurationModel<TOptions>() where TOptions : class
        {
            _serviceCollectionBuilder.Services.Configure<TOptions>(_context.Configuration);
            return this;
        }

        public KernelBuilder UseLogger(Action<KernelBuilderContext, ILoggingBuilder> configureDelegate)
        {
            _useLogger = true;
            configureDelegate(_context, _serviceCollectionBuilder);
            return this;
        }

        public KernelBuilder Configure(Action<KernelBuilderContext, ContainerBuilder> configureDelegate)
        {
            buildActions.Add(() => configureDelegate(_context, _containerBuilder));
            return this;
        }

        public KernelBuilder UseContentDirectory(string root)
        {
            _hasRegisterContentRoot = true;
            buildActions.Add(() => _containerBuilder.RegisterInstance(new ContentRoot(root)).As<IContentRoot>());
            return this;
        }

        public Kernel Build()
        {
            if (_useLogger)
            {
                _serviceCollectionBuilder.Services.AddLogging();
            }

            _containerBuilder.Populate(_serviceCollectionBuilder.Services);
            // Autofac compability
            if (!_hasRegisterContentRoot)
            {
                _containerBuilder.RegisterInstance(new ContentRoot(Directory.GetCurrentDirectory())).As<IContentRoot>();
            }
            buildActions.ForEach((builder) => builder());
            return new Kernel(_containerBuilder);
        }
    }
}
