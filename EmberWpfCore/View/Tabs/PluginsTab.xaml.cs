using Autofac;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public static readonly DependencyProperty IsCurrentSelectionPluginInitializedProperty =
            DependencyProperty.Register("IsCurrentSelectionPluginInitialized", typeof(bool), typeof(IPluginManagerViewModel), new PropertyMetadata(true));

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
                this.ViewModel = new PluginsTabViewModel
                {
                    PluginsViewModel = pluginsViewModel
                };
                this.DataContext = pluginsViewModel;
            }
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
        private IPluginManagerViewModel pluginsViewModel;
        public IPluginManagerViewModel PluginsViewModel
        {
            get => pluginsViewModel;
            set
            {
                if (Equals(pluginsViewModel, value)) return;
                pluginsViewModel = PluginsViewModel;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
