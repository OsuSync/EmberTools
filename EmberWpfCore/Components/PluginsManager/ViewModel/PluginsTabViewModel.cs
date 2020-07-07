using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using EmberWpfCore.Components.PluginsManager.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EmberWpfCore.Components.PluginsManager.ViewModel
{
    public enum PluginStatus
    {
        /*Add, Remove,*/
        Initialized, Uninitialized
    }

    public class PluginsTabViewModel : INotifyPropertyChanged
    {
        private PluginDescriptor _selectedPlugin;
        private readonly DisablePluginCommand disablePluginCommand;
        private readonly EnablePluginCommand enablePluginCommand;
        public ICommand DisablePluginCommand => disablePluginCommand;
        public ICommand EnablePluginCommand => enablePluginCommand;

        public PluginsTabViewModel(IPluginManagerViewModel pluginManagerViewModel)
        {
            PluginsViewModel = pluginManagerViewModel;
            disablePluginCommand = new DisablePluginCommand(pluginManagerViewModel);
            enablePluginCommand = new EnablePluginCommand(pluginManagerViewModel);
            PluginsViewModel.PropertyChanged += PluginsViewModel_PropertyChanged;
        }

        private void PluginsViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            disablePluginCommand.OnCanExecuteChanged();
            enablePluginCommand.OnCanExecuteChanged();
        }

        public IPluginManagerViewModel PluginsViewModel { get; set; }

        public PluginDescriptor SelectedPlugin
        {
            get => _selectedPlugin;
            set
            {
                if (Equals(_selectedPlugin, value)) return;
                _selectedPlugin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedPluginStatus));
                disablePluginCommand.OnCanExecuteChanged();
                enablePluginCommand.OnCanExecuteChanged();
            }
        }

        public PluginStatus SelectedPluginStatus => SelectedPlugin == null ? PluginStatus.Uninitialized : PluginsViewModel.IsPluginInitialized(SelectedPlugin)
            ? PluginStatus.Initialized
            : PluginStatus.Uninitialized;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
