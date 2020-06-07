using EmberKernel.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Model.Configuration
{
    public class ConfigurationModelManager : ObservableCollection<object>, IConfigurationModelManager
    {
        public void Add<TPlugin, TOptions>(ConfigurationDependencyObject<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            dependencyObject.PropertyChanged += DependencyObject_PropertyChanged;
            this.Add(dependencyObject);
        }

        private void DependencyObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        public void Remove<TPlugin, TOptions>(ConfigurationDependencyObject<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            dependencyObject.PropertyChanged -= DependencyObject_PropertyChanged;
            base.Remove(dependencyObject);
        }
    }
}
