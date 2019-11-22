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
        public static void UseConfigurationModel<TOptions>(
            this IComponentBuilder builder,
            Func<IConfiguration, IConfiguration> configurationSelector)
            where TOptions : class, new()
        {
            var _builder = builder as ComponentBuilder;
            var _conf = _builder.ParentScope.Resolve<IConfiguration>();
            var config = configurationSelector(_conf);

            _builder.Container.Configure<TOptions>(config);
        }


        public static void UseConfigurationModel<TOptions>(this IComponentBuilder builder) where TOptions : class, new()
            => UseConfigurationModel<TOptions>(builder, conf => conf.GetSection(typeof(TOptions).Name));
    }
}
