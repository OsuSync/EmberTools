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
            public IConfiguration Configuration { get; }
        }

        internal class LoggingBuilder : ILoggingBuilder
        {
            public IServiceCollection Services { get; }
            public LoggingBuilder(IServiceCollection services)
            {
                Services = services;
            }
        }

        internal ContainerBuilder _containerBuilder;
        internal IConfigurationBuilder _configurationBuilder;
        internal LoggingBuilder _loggingBuilder;
        internal KernelBuilderContext _context;
        internal bool _hasRegisterContentRoot = false;
        public KernelBuilder()
        {
            _containerBuilder = new ContainerBuilder();
            _configurationBuilder = new ConfigurationBuilder();
            _context = new KernelBuilderContext();
            _loggingBuilder = new LoggingBuilder(new ServiceCollection());
            _containerBuilder.Populate(_loggingBuilder.Services);
        }

        public KernelBuilder UsePlugins<TPluginsLoader>() where TPluginsLoader : IPluginsLoader
        {
            _containerBuilder.RegisterType<PluginsLayer<TPluginsLoader>>().As<IPluginsLayer>();
            return this;
        }

        public KernelBuilder UseKernalService<TKernelService>() where TKernelService : KernelService
        {
            _containerBuilder.RegisterType<TKernelService>();
            return this;
        }

        public KernelBuilder UseConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            configureDelegate(_configurationBuilder);
            _containerBuilder.RegisterInstance(_configurationBuilder.Build());
            return this;
        }

        public KernelBuilder UseConfigurationModel<TOptions>() where TOptions : class
        {
            _loggingBuilder.Services.Configure<TOptions>(_context.Configuration);
            return this;
        }

        public KernelBuilder UseLogger(Action<KernelBuilderContext, ILoggingBuilder> configureDelegate)
        {
            configureDelegate(_context, _loggingBuilder);
            return this;
        }

        public KernelBuilder Configure(Action<KernelBuilderContext, ContainerBuilder> configureDelegate)
        {
            configureDelegate(_context, _containerBuilder);
            return this;
        }

        public KernelBuilder UseContentDirectory(string root)
        {
            _hasRegisterContentRoot = true;
            _containerBuilder.RegisterInstance(new ContentRoot(root)).As<IContentRoot>();
            return this;
        }

        public Kernel Build()
        {
            if (!_hasRegisterContentRoot)
            {
                _containerBuilder.RegisterInstance(new ContentRoot(Directory.GetCurrentDirectory())).As<IContentRoot>();
            }
            return new Kernel(_containerBuilder);
        }
    }
}
