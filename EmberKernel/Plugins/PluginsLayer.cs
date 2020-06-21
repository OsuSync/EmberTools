﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;

namespace EmberKernel.Plugins
{
    public class PluginsLayer<T> : IPluginsLayer where T : IPluginsLoader
    {
        public void BuildScope(ContainerBuilder builder)
        {
            var pluginManBuilder = builder.RegisterType<T>().As<IPluginsLoader>().SingleInstance();
            if (typeof(IPluginsManager).IsAssignableFrom(typeof(T)))
            {
                pluginManBuilder.As<IPluginsManager>().SingleInstance();
                builder.RegisterType<PluginManagerViewModel>().As<IPluginManagerViewModel>().SingleInstance();
            }
        }

        public async Task Run(ILifetimeScope scope)
        {
            var pluginLoader = scope.Resolve<IPluginsLoader>();
            using var pluginsScope = scope.BeginLifetimeScope(builder =>
            {
                pluginLoader.BuildScope(builder);
            });

            await pluginLoader.Run(pluginsScope);
            await pluginLoader.RunEntryComponents();
        }
    }
}
