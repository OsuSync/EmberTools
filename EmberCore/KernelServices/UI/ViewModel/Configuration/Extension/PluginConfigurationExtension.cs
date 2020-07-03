using Autofac;
using EmberCore.KernelServices.UI.ViewModel.Configuration;
using EmberKernel.Plugins;
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
        public static void RegisterUIModel<TPlugin, TOptions>(this ILifetimeScope scope, Action<WpfFeatureBuilder<TPlugin, TOptions>> configure)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            var windowManager = scope.Resolve<IWindowManager>();
            var dependencyObject = scope.Resolve<ConfigurationDependencySet<TPlugin, TOptions>>();
            var settingManager = scope.Resolve<IConfigurationModelManager>();
            configure(new WpfFeatureBuilder<TPlugin, TOptions>(dependencyObject));
            windowManager.BeginUIThreadScope(() => settingManager.Add(dependencyObject));
        }
    }
}
