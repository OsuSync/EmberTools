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

        DependencySet Dependency { get; set; }
        public ConfigurationViewModel(DependencySet dependency)
        {
            Dependency = dependency;
            Dependency.PropertyChanged += Dependency_PropertyChanged;
        }

        private void Dependency_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public string LatestBeatmapFile
        {
            get => (string)Dependency.GetValue(nameof(LatestBeatmapFile), DependencySet.NameDependencies[nameof(LatestBeatmapFile)]);
            set => Dependency.SetValue(nameof(LatestBeatmapFile), DependencySet.NameDependencies[nameof(LatestBeatmapFile)], value);
        }

        public int MyIntValue
        {
            get => (int)Dependency.GetValue(nameof(MyIntValue), DependencySet.NameDependencies[nameof(MyIntValue)]);
            set => Dependency.SetValue(nameof(MyIntValue), DependencySet.NameDependencies[nameof(MyIntValue)], value);
        }

    }
}
