using Autofac;
using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins
{
    public static class PluginConfigurationExtension
    {
        public static void UseConfigurationModel<TPlugin, TOptions>(
            this IComponentBuilder builder,
            Func<IConfiguration, IConfiguration> configurationSelector)
            where TOptions : class, new()
            where TPlugin : Plugin
        {
            var _builder = builder as ComponentBuilder;
            var _conf = _builder.ParentScope.Resolve<IConfiguration>();
            var config = configurationSelector(_conf);

            _builder.Container.Configure<TPlugin, TOptions>(config);
        }


        public static void UseConfigurationModel<TPlugin, TOptions>(this IComponentBuilder builder)
            where TOptions : class, new()
            where TPlugin : Plugin
            => UseConfigurationModel<TPlugin, TOptions>(builder, conf => conf.GetSection($"{typeof(TPlugin).Namespace}.{typeof(TPlugin).Name}.{typeof(TOptions).Name}"));
    }
}
