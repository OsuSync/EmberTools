using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace EmberKernel.Plugins
{
    public static class PluginConfigurationExtension
    {
        public static void UseConfigurationModel<TOptions>(
            this IComponentBuilder builder,
            Func<IConfiguration, IConfiguration> configurationSelector,
            string @namespace)
            where TOptions : class, new()
        {
            var _builder = builder as ComponentBuilder;
            var _conf = _builder.ParentScope.Resolve<IConfiguration>();
            var config = configurationSelector(_conf);

            _builder.Container.Configure<TOptions>(config, @namespace);
        }

        public static void UseConfigurationModel<TOptions>(this IComponentBuilder builder, string pluginName)
            where TOptions : class, new()
        {
            var @namespace = $"{pluginName}.{typeof(TOptions).Name}";
            builder.Container.RegisterInstance(pluginName).Named<string>(ReadOnlyPluginOptions<TOptions>.PLUGIN_NAME);
            builder.Container.RegisterType<ReadOnlyPluginOptions<TOptions>>().As<IReadOnlyPluginOptions<TOptions>>().SingleInstance();
            UseConfigurationModel<TOptions>(builder, conf => conf.GetSection(@namespace), @namespace);
        }

        public static void UsePluginOptionsModel<TPlugin, TOptions>(this IComponentBuilder builder)
            where TOptions : class, new()
            where TPlugin : Plugin
        {
            builder.Container.RegisterType<PluginOptions<TPlugin, TOptions>>().As<IPluginOptions<TPlugin, TOptions>>().SingleInstance();
            UseConfigurationModel<TOptions>(builder, typeof(TPlugin).Name);
        }
    }
}
