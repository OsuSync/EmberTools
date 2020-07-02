using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using EmberKernel;

namespace EmberWpfCore.Components.Configuration.ViewModel
{
    public class ConfigurationSetViewModel : ObservableCollection<ConfigurationItemViewModel>
    {
        public Type ConfigurationType { get; }
        public string Name { get; }
        public IReadOnlyList<ConfigurationItemViewModel> ConfigurationItems { get; private set; }
        public DependencySet DependencySet { get; }
        private Dictionary<string, ConfigurationItemViewModel> ViewModels { get; }
            = new Dictionary<string, ConfigurationItemViewModel>();
        public ConfigurationSetViewModel(DependencySet dependencySet)
        {
            DependencySet = dependencySet;
            var type = DependencySet.GetType();
            ConfigurationType = type;
            var nameAttr = ConfigurationType.GetCustomAttribute<ConfigurationSetNameAttribute>();
            if (nameAttr != null)
            {
                Name = nameAttr.Name;
            }
            else
            {
                Name = type.GetGenericName(1);
            }
        }

        public void LoadDependencySet()
        {
            var properties = new List<ConfigurationItemViewModel>();
            foreach (var (name, method) in DependencySet.Dependency.NameDependencies)
            {
                var vm = new ConfigurationItemViewModel(method.PropertyType, name, DependencySet);
                ViewModels.Add(name, vm);
                Add(vm);
            }
            ConfigurationItems = properties;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
