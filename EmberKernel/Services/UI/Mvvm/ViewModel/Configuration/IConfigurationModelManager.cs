using EmberKernel.Plugins;
using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.Dependency.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Configuration
{
    public interface IConfigurationModelManager : IEnumerable<object>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        void Add<TPlugin, TOptions>(ConfigurationDependencySet<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new();

        void Remove<TPlugin, TOptions>(ConfigurationDependencySet<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new();

        DependencySet GetDependency(string name);
    }
}
