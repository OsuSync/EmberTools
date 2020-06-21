using Autofac;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EmberWpfCore.View.Tabs
{
    /// <summary>
    /// Interaction logic for PluginsTab.xaml
    /// </summary>
    [ViewComponentNamespace(@namespace: "CoreWpfTab")]
    public partial class PluginsTab : UserControl, IViewComponent
    {
        public PluginsTab()
        {
            InitializeComponent();
        }

        private PluginsTabViewModel ViewModel { get; set; }
        //private IPluginManagerViewModel ViewModel { get; set; }
        public Task Initialize(ILifetimeScope scope)
        {
            var logger = scope.Resolve<ILogger<PluginsTab>>();
            if (scope.TryResolve<IPluginManagerViewModel>(out var pluginsViewModel))
            {
                // 在这里设置了DataContext
                // xaml里bind
                // IPluginManagerViewModel是一个ObserverCollection<T>
                //this.DataContext = pluginsViewModel;
                //this.ViewModel = pluginsViewModel;
                // pluginsViewModel.IsPluginInitialized(descriptor);
                //this.ViewModel = new PluginsTabViewModel(pluginsViewModel);
                this.ViewModel = new PluginsTabViewModel(pluginsViewModel);
                this.DataContext = this.ViewModel;
            }
            else
                logger.LogError("Can't resolve 'IPluginManagerViewModel'");
            return Task.CompletedTask;
        }

        public Task Uninitialize(ILifetimeScope scope)
        {
            this.DataContext = null;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }

    class PluginsTabViewModel : INotifyPropertyChanged
    {
        private PluginDescriptor _selectedPlugin;

        public PluginsTabViewModel(IPluginManagerViewModel pluginManagerViewModel)
        {
            PluginsViewModel = pluginManagerViewModel;
        }

        public IPluginManagerViewModel PluginsViewModel { get; }

        public PluginDescriptor SelectedPlugin
        {
            get => _selectedPlugin;
            set
            {
                if (Equals(_selectedPlugin, value)) return;
                _selectedPlugin = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedPluginStatus));
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
