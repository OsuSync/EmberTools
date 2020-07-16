using Autofac;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Plugins;
using EmberWpfCore.Components.PluginsManager.ViewModel;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EmberWpfCore.Components.PluginsManager.View
{
    /// <summary>
    /// Interaction logic for PluginsTab.xaml
    /// </summary>
    [ViewComponentNamespace(@namespace: "CoreWpfTab")]
    [ViewComponentName(name: "Plugins")]
    public partial class PluginsTab : UserControl, IViewComponent
    {
        public PluginsTab()
        { 
            InitializeComponent();
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            var logger = scope.Resolve<ILogger<PluginsTab>>();
            if (scope.TryResolve<IPluginManagerViewModel>(out var pluginsViewModel))
            {
                this.DataContext = new PluginsTabViewModel(pluginsViewModel);
            }
            else
                logger.LogError("Can't resolve 'IPluginManagerViewModel'");
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            this.DataContext = null;
            return default;
        }

        public void Dispose()
        {
        }
    }
}
