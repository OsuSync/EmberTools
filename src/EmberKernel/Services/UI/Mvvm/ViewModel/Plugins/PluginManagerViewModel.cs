using EmberKernel.Plugins;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Plugins
{
    public class PluginManagerViewModel : ObservableCollection<PluginDescriptor>, IPluginManagerViewModel, IDisposable
    {
        private readonly IDisposable _onPluginLoad;
        private readonly IDisposable _onPluginUnload;
        private readonly IDisposable _onPluginInitialized;
        private IWindowManager WindowManager { get; }
        private IPluginsManager PluginsManager { get; }

        public PluginManagerViewModel(IPluginsManager pluginsManager, IWindowManager windowManager)
        {
            WindowManager = windowManager;
            PluginsManager = pluginsManager;
            InitializePlugins();
            _onPluginLoad = PluginsManager.OnPluginLoad((descriptor) =>
            {
                AddPlugin(descriptor);
                WindowManager.BeginUIThreadScope(() => OnPropertyChanged(new PropertyChangedEventArgs(descriptor.ToString())));

            });
            _onPluginUnload = PluginsManager.OnPluginUnload((descriptor) =>
            {
                WindowManager.BeginUIThreadScope(() => OnPropertyChanged(new PropertyChangedEventArgs(descriptor.ToString())));
            });
            _onPluginInitialized = PluginsManager.OnPluginInitialized((descriptor) =>
            {
                WindowManager.BeginUIThreadScope(() => OnPropertyChanged(new PropertyChangedEventArgs(descriptor.ToString())));
            });
        }

        private void InitializePlugins()
        {
            foreach (var item in PluginsManager.LoadedPlugins())
            {
                AddPlugin(item);
            }
        }
        
        public void DisablePlugin(PluginDescriptor item)
        {
            if (PluginsManager.GetPluginByDescriptor(item) is IPlugin plugin)
            {
                PluginsManager.Unload(plugin);
            }
        }

        public void EnablePlugin(PluginDescriptor item)
        {
            if (PluginsManager.GetPluginByDescriptor(item) is IPlugin plugin)
            {
                PluginsManager.Load(plugin);
                PluginsManager.Initialize(plugin);
            }
        }

        public bool IsPluginInitialized(PluginDescriptor item)
        {
            if (PluginsManager.GetPluginByDescriptor(item) is IPlugin plugin)
            {
                return PluginsManager.IsPluginInitialized(plugin);
            }
            return false;
        }

        private void AddPlugin(PluginDescriptor item)
        {
            
            WindowManager.BeginUIThreadScope(() =>
            {
                if (this.Any(desc => desc.ToString() == item.ToString())) return;
                Add(item);
            });
        }

        public void Dispose()
        {
            _onPluginLoad.Dispose();
            _onPluginUnload.Dispose();
            _onPluginInitialized.Dispose();
        }
    }
}
