using EmberKernel.Services.UI.Mvvm.Dependency;
using System.ComponentModel;

namespace EmberWpfCore.Components.Configuration.ViewModel
{
    public class ConfigurationSingleValueViewModel : INotifyPropertyChanged
    {
        public object Value
        {
            get => DependencySet[Name];
            set
            {
                DependencySet[Name] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
            }
        }

        private string Name { get; }
        private DependencySet DependencySet { get; }

        public event PropertyChangedEventHandler PropertyChanged;


        public ConfigurationSingleValueViewModel(string name, DependencySet dependencySet)
        {
            this.Name = name;
            this.DependencySet = dependencySet;
            DependencySet.PropertyChanged += DependencySet_PropertyChanged;
        }

        private void DependencySet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }
}
