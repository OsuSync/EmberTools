using Autofac;
using Autofac.Extensions.DependencyInjection;
using EmberKernel.Plugins;
using EmberKernel.Services;
using EmberKernel.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            buildActions.Add(() => _containerBuilder.RegisterType<PluginsLayer<TPluginsLoader>>().As<IPluginsLayer>().SingleInstance());
            return this;
        }

        public KernelBuilder UseKernelService<TKernelService>() where TKernelService : IKernelService
        {
            buildActions.Add(() => _containerBuilder.RegisterType<TKernelService>().AsSelf().As<TKernelService>().SingleInstance());
            return this;
        }


        public KernelBuilder UseKernelService<TKernelService, TIKernelService>(bool autoActive = false) where TKernelService : IKernelService, TIKernelService
        {
            buildActions.Add(() =>
            {
                var registertion = _containerBuilder.RegisterType<TKernelService>().AsSelf().As<TIKernelService>().SingleInstance();
                if (autoActive) registertion.AutoActivate();
            });
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

        public KernelBuilder UsePluginOptions(string persistFilePath)
        {
            buildActions.Add(() =>
            {
                _containerBuilder.RegisterInstance(new PluginOptionsSetting() { PersistFilePath = persistFilePath }).SingleInstance();
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
