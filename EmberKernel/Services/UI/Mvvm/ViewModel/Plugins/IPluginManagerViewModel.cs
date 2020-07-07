using EmberKernel.Plugins.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Plugins
{
    public interface IPluginManagerViewModel : INotifyCollectionChanged, INotifyPropertyChanged, ICollection<PluginDescriptor>
    {
        void DisablePlugin(PluginDescriptor item);
        void EnablePlugin(PluginDescriptor item);
        bool IsPluginInitialized(PluginDescriptor item);
    }

}
