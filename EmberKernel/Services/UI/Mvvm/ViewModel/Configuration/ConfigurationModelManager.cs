using EmberKernel.Plugins;
using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.Dependency.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public void Add<TPlugin, TOptions>(ConfigurationDependencySet<TPlugin, TOptions> dependencyObject)
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

        public void Remove<TPlugin, TOptions>(ConfigurationDependencySet<TPlugin, TOptions> dependencyObject)
            where TPlugin : Plugin
            where TOptions : class, new()
        {
            dependencyObject.PropertyChanged -= DependencyObject_PropertyChanged;
            base.Remove(dependencyObject);
        }

        public DependencySet GetDependency(string name)
        {
            foreach (object dependency in this)
            {
                if (dependency is DependencySet dc)
                {
                    if (dc.GetType().GetGenericName(1) == name)
                    {
                        return dc;
                    }
                }
            }
            return null;
        }

        public void Dispose()
        {
            manager.Unregister<ConfigurationModelManager>();
        }
    }
}
