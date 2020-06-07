using EmberKernel.Plugins;
using EmberKernel.Services.UI.Mvvm.Dependency.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Configuration
{
    public interface IConfigurationModelManager : INotifyCollectionChanged, INotifyPropertyChanged
    {
        void Add<TPlugin, TOptions>(ConfigurationDependencyObject<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new();

        void Remove<TPlugin, TOptions>(ConfigurationDependencyObject<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new();
    }
}
