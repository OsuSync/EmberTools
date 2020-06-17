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
        public ConfigurationViewModel(DependencySet dependency)
        {
            DependencySet = dependency;
            DependencySet.PropertyChanged += Dependency_PropertyChanged;
        }

        private void Dependency_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        public string LatestBeatmapFile
        {
            get => (string)DependencySet[nameof(LatestBeatmapFile)];
            set => DependencySet[nameof(LatestBeatmapFile)] = value;
        }

        public int MyIntValue
        {
            get => (int)DependencySet[nameof(MyIntValue)];
            set => DependencySet[nameof(MyIntValue)] = value;
        }

    }
}
