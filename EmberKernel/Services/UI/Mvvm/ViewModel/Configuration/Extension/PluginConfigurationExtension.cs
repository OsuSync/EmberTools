using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Dependency.Configuration;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Extension
{
    public static class PluginConfigurationExtension
    {
        public static void ConfigureUIModel<TPlugin, TOptions>(this IComponentBuilder builder)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            builder.Container.RegisterType<ConfigurationDependencyObject<TPlugin, TOptions>>();
        }

        public static void RegisterUIModel<TPlugin, TOptions>(this ILifetimeScope scope)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            var dependencyObject = scope.Resolve<ConfigurationDependencyObject<TPlugin, TOptions>>();
            var settingManager = scope.Resolve<IConfigurationModelManager>();
            settingManager.Add(dependencyObject);
        }

        public static void UnregisterUIModel<TPlugin, TOptions>(this ILifetimeScope scope)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            var settingManager = scope.Resolve<IConfigurationModelManager>();
            var dependencyObject = scope.Resolve<ConfigurationDependencyObject<TPlugin, TOptions>>();
            settingManager.Remove(dependencyObject);
        }
    }
}
