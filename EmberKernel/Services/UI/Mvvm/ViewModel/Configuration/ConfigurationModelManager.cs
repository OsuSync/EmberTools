using EmberKernel.Plugins;
using EmberKernel.Services.UI.Mvvm.Dependency.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Configuration
{
    public class ConfigurationModelManager : ObservableCollection<object>, INotifyPropertyChanged, IConfigurationModelManager, IDisposable
    {
        public new event PropertyChangedEventHandler PropertyChanged;
        private IViewModelManager manager;
        public ConfigurationModelManager(IViewModelManager manager)
        {
            this.manager = manager;
            this.manager.Register(this);
            base.PropertyChanged += DependencyObject_PropertyChanged;
        }

        public void Add<TPlugin, TOptions>(ConfigurationDependencyObject<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            dependencyObject.PropertyChanged += DependencyObject_PropertyChanged;
            base.Add(dependencyObject);
        }

        private void DependencyObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        public void Remove<TPlugin, TOptions>(ConfigurationDependencyObject<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            dependencyObject.PropertyChanged -= DependencyObject_PropertyChanged;
            base.Remove(dependencyObject);
        }

        public void Dispose()
        {
            manager.Unregister<ConfigurationModelManager>();
        }
    }
}
