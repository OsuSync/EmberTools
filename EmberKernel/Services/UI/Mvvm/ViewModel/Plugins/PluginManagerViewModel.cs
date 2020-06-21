using EmberKernel.Plugins;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Plugins
{
    public class PluginManagerViewModel : ObservableCollection<PluginDescriptor>, IPluginManagerViewModel, IDisposable
    {
        private readonly IDisposable _onPluginLoad;
        private readonly IDisposable _onPluginUnload;
        private readonly IDisposable _onPluginStatusUpdated;
        private IWindowManager WindowManager { get; }
        private IPluginsManager PluginsManager { get; }
        private readonly DisablePluginCommand disablePluginCommand;
        private readonly EnablePluginCommand enablePluginCommand;
        public ICommand DisablePluginCommand => disablePluginCommand;
        public ICommand EnablePluginCommand => enablePluginCommand;

        public event Action<PluginDescriptor, PluginStatus> PluginStatusChanged;

        public PluginManagerViewModel(IPluginsManager pluginsManager, IWindowManager windowManager)
        {
            WindowManager = windowManager;
            PluginsManager = pluginsManager;
            disablePluginCommand = new DisablePluginCommand(this);
            enablePluginCommand = new EnablePluginCommand(this);
            InitializePlugins();
            _onPluginLoad = pluginsManager.OnPluginLoad((descriptor) => AddPlugin(descriptor));
            _onPluginUnload = pluginsManager.OnPluginUnload((desciptor) => RemovePlugin(desciptor));
            _onPluginStatusUpdated = pluginsManager.OnPluginStatusUpdated(PluginStatusUpdated);
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
                PluginsManager.Unload(plugin).Wait();
            }
        }

        public void EnablePlugin(PluginDescriptor item)
        {
            if (PluginsManager.GetPluginByDescriptor(item) is IPlugin plugin)
            {
                PluginsManager.Load(plugin).Wait();
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
            WindowManager.BeginUIThreadScope(() => Add(item));
            //PluginStatusChanged?.Invoke(???,???);
        }

        private void RemovePlugin(PluginDescriptor item)
        {
            WindowManager.BeginUIThreadScope(() => Remove(item));
            //PluginStatusChanged?.Invoke(???,???);
        }

        private void PluginStatusUpdated(PluginDescriptor item)
        {
            //PluginStatusChanged?.Invoke(???,???);
        }

        public void Dispose()
        {
            _onPluginLoad.Dispose();
            _onPluginUnload.Dispose();
            _onPluginStatusUpdated.Dispose();
        }
    }
}
