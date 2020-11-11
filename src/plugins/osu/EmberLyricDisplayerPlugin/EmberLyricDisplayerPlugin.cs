using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Configuration;
using EmberLyricDisplayerPlugin.Kernel;
using LyricsFinder;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmberLyricDisplayerPlugin
{
    [EmberPlugin(Author = "MikiraSora", Name = "EmberLyricDisplayerPlugin", Version = "0.7.5")]
    public class EmberLyricDisplayerPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<Kernel.LyricsFinder>().SingleInstance();

            builder.UsePluginOptionsModel<EmberLyricDisplayerPlugin, PluginOptions>();
            builder.UseConfigurationModel<PluginOptions>("EmberLyricDisplayerPlugin");
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            // init or update config
            var optFactory = scope.Resolve<IPluginOptions<EmberLyricDisplayerPlugin,PluginOptions>>();
            var opt = optFactory.Create();

            var logger = scope.Resolve<ILogger<EmberLyricDisplayerPlugin>>();
            var lyricsFinder = scope.Resolve<Kernel.LyricsFinder>();

            //apply part of plugins options to lyrics option
            GlobalSetting.DebugMode = opt.DebugMode;
            GlobalSetting.SearchAndDownloadTimeout = opt.SearchAndDownloadTimeout;
            GlobalSetting.StrictMatch = opt.StrictMatch;
            GlobalSetting.OutputFunc = msg => {
                logger.LogInformation(msg);
            };

            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            return default;
        }
    }
}
