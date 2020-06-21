using EmberKernel.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Plugins
{
    public interface IPluginManagerViewModel : INotifyCollectionChanged, INotifyPropertyChanged, ICollection<PluginDescriptor>
    {
        void DisablePlugin(PluginDescriptor item);
        void EnablePlugin(PluginDescriptor item);
        bool IsPluginInitialized(PluginDescriptor item);
        ICommand DisablePluginCommand { get; }
        ICommand EnablePluginCommand { get; }
    }
}
