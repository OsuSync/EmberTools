using EmberKernel.Plugins;
using EmberKernel.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EmberKernel.Services.UI.Mvvm.ViewModel.Plugins.Commands
{
    class DisablePluginCommand : ICommand
    {
        public PluginManagerViewModel PluginManagerViewModel { get; set; }
        public DisablePluginCommand(PluginManagerViewModel pluginsManager)
        {
            PluginManagerViewModel = pluginsManager;
        }

#pragma warning disable CS0067 // #warning directive
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // #warning directive

        public bool CanExecute(object parameter)
        {
            return true;
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
