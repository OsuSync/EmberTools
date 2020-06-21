using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Dependency.Configuration;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
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
            builder.Container.RegisterType<ConfigurationDependencySet<TPlugin, TOptions>>().SingleInstance();
        }

        public static void RegisterUIModel<TPlugin, TOptions>(this ILifetimeScope scope)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            var windowManager = scope.Resolve<IWindowManager>();
            var dependencyObject = scope.Resolve<ConfigurationDependencySet<TPlugin, TOptions>>();
            var settingManager = scope.Resolve<IConfigurationModelManager>();
            windowManager.BeginUIThreadScope(() => settingManager.Add(dependencyObject));
        }

        public static void UnregisterUIModel<TPlugin, TOptions>(this ILifetimeScope scope)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            var windowManager = scope.Resolve<IWindowManager>();
            var settingManager = scope.Resolve<IConfigurationModelManager>();
            var dependencyObject = scope.Resolve<ConfigurationDependencySet<TPlugin, TOptions>>();
            windowManager.BeginUIThreadScope(() => settingManager.Remove(dependencyObject));
        }
    }
}
