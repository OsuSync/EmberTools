using EmberKernel.Services.UI.Mvvm.Dependency;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EmberWpfCore.ViewModel
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        DependencySet DependencySet { get; set; }
        private TypeDependency Dependency { get; }
        public ConfigurationViewModel(DependencySet dependency)
        {
            DependencySet = dependency;
            DependencySet.PropertyChanged += Dependency_PropertyChanged;
            Dependency = DependencySet.Dependency;
        }

        private void Dependency_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public string LatestBeatmapFile
        {
            get => (string)DependencySet.GetValue(nameof(LatestBeatmapFile), Dependency.NameDependencies[nameof(LatestBeatmapFile)]);
            set => DependencySet.SetValue(nameof(LatestBeatmapFile), Dependency.NameDependencies[nameof(LatestBeatmapFile)], value);
        }

        public int MyIntValue
        {
            get => (int)DependencySet.GetValue(nameof(MyIntValue), Dependency.NameDependencies[nameof(MyIntValue)]);
            set => DependencySet.SetValue(nameof(MyIntValue), Dependency.NameDependencies[nameof(MyIntValue)], value);
        }

    }
}
