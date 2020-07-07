using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using System;
using System.Windows.Input;

namespace EmberWpfCore.Components.PluginsManager.Commands
{
    class DisablePluginCommand : ICommand
    {
        public IPluginManagerViewModel PluginManagerViewModel { get; set; }
        public DisablePluginCommand(IPluginManagerViewModel pluginsManager)
        {
            PluginManagerViewModel = pluginsManager;
        }

        public event EventHandler CanExecuteChanged;
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter)
        {
            if (parameter is PluginDescriptor descriptor)
            {
                return PluginManagerViewModel.IsPluginInitialized(descriptor);
            }
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is PluginDescriptor descriptor)
            {
                PluginManagerViewModel.DisablePlugin(descriptor);
            }
        }
    }
}
