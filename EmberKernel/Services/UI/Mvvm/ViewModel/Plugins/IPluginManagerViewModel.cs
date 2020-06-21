using EmberKernel.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Plugins
{
    public interface IPluginManagerViewModel : INotifyCollectionChanged, INotifyPropertyChanged, ICollection<PluginDescriptor>
    {
        event Action<PluginDescriptor, PluginStatus> PluginStatusChanged;
        void DisablePlugin(PluginDescriptor item);
        void EnablePlugin(PluginDescriptor item);
        bool IsPluginInitialized(PluginDescriptor item);
        ICommand DisablePluginCommand { get; }
        ICommand EnablePluginCommand { get; }
    }

    public enum PluginStatus
    {
        /*Add, Remove,*/Initialized, Uninitialized
    }
}
