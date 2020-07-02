using EmberKernel.Services.UI.Mvvm.Dependency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EmberWpfCore.Components.Configuration.ViewModel
{
    public class ConfigurationItemViewModel : INotifyPropertyChanged
    {
        public object Value { get => DependencySet.GetValue(Name);
            set
            {
                DependencySet.SetValue(Name, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
            }
        }
        public string Name { get; }
        public Type ValueType { get; }
        private DependencySet DependencySet { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ConfigurationItemViewModel(Type propertyType, string name, DependencySet dependencySet)
        {
            Name = name;
            ValueType = propertyType;
            DependencySet = dependencySet;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
