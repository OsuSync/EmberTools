using Autofac;
using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EmberKernel.Services.Configuration
{
    public static class PluginOptionsExtensions
    {
        private static IComponentBuilder AddOptions<TOptions>(this IComponentBuilder builder) where TOptions : class, new()
        {
            var services = builder.Container;
            services.RegisterType<OptionsManager<TOptions>>().As<IOptions<TOptions>>().SingleInstance();
            services.RegisterType<OptionsManager<TOptions>>().As<IOptionsSnapshot<TOptions>>().InstancePerLifetimeScope();
            services.RegisterType<OptionsMonitor<TOptions>>().As<IOptionsMonitor<TOptions>>().SingleInstance();
            services.RegisterType<OptionsFactory<TOptions>>().As<IOptionsFactory<TOptions>>().InstancePerDependency();
            services.RegisterType<OptionsCache<TOptions>>().As<IOptionsMonitorCache<TOptions>>().SingleInstance();
            return builder;
        }

        public static IComponentBuilder Configure<TOptions>(this IComponentBuilder builder, IConfiguration config, string @namespace)
            where TOptions : class, new()
        {
            builder.AddOptions<TOptions>();
            var services = builder.Container;
            services
                .RegisterInstance(new ConfigurationChangeTokenSource<TOptions>(@namespace, config))
                .SingleInstance().As<IOptionsChangeTokenSource<TOptions>>();
            services
                .RegisterInstance(new NamedConfigureFromConfigurationOptions<TOptions>(@namespace, config))
                .SingleInstance()
                .As<IConfigureNamedOptions<TOptions>>()
                .As<IConfigureOptions<TOptions>>()
                .As<IConfigureNamedOptions<TOptions>>();
            return builder;
        }

    }
}
