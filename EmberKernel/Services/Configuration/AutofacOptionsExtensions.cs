using EmberKernel.Plugins;
using EmberKernel.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autofac
{
    /// <summary>
    /// Autofac port for M$ Options Extension
    /// see
    /// https://github.com/aspnet/Extensions/blob/master/src/Options/Options/src/OptionsServiceCollectionExtensions.cs
    /// https://github.com/aspnet/Extensions/blob/master/src/Options/ConfigurationExtensions/src/OptionsConfigurationServiceCollectionExtensions.cs
    /// </summary>
    public static class AutofacOptionsExtensions
    {
        private static ContainerBuilder AddOptions<TOptions>(this ContainerBuilder services) where TOptions : class, new()
        {
            services.RegisterType<OptionsManager<TOptions>>().As<IOptions<TOptions>>().SingleInstance();
            services.RegisterType<OptionsManager<TOptions>>().As<IOptionsSnapshot<TOptions>>().InstancePerLifetimeScope();
            services.RegisterType<OptionsMonitor<TOptions>>().As<IOptionsMonitor<TOptions>>().SingleInstance();
            services.RegisterType<OptionsFactory<TOptions>>().As<IOptionsFactory<TOptions>>().InstancePerDependency();
            services.RegisterType<OptionsCache<TOptions>>().As<IOptionsMonitorCache<TOptions>>().SingleInstance();
            return services;
        }

        public static ContainerBuilder Configure<TPlugin, TOptions>(this ContainerBuilder builder, IConfiguration config)
            where TOptions : class, new()
            where TPlugin : Plugin
        {
            var @namespace = $"{typeof(TPlugin).Namespace}.{typeof(TPlugin).Name}.{typeof(TOptions).Name}";
            builder.AddOptions<TOptions>();
            builder.RegisterType<OptionsModerator<TPlugin, TOptions>>().As<IOptionsModerator<TPlugin, TOptions>>().SingleInstance();
            builder
                .RegisterInstance(new ConfigurationChangeTokenSource<TOptions>(@namespace, config))
                .SingleInstance().As<IOptionsChangeTokenSource<TOptions>>();
            builder
                .RegisterInstance(new NamedConfigureFromConfigurationOptions<TOptions>(@namespace, config))
                .SingleInstance()
                .As<IConfigureNamedOptions<TOptions>>()
                .As<IConfigureOptions<TOptions>>()
                .As<IConfigureNamedOptions<TOptions>>();
            return builder;
        }
    }
}
