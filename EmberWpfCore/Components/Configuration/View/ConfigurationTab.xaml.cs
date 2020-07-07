using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using EmberWpfCore.Components.Configuration.ViewModel;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EmberWpfCore.Components.Configuration.View
{
    /// <summary>
    /// Interaction logic for ConfigurationTab.xaml
    /// </summary>
    [ViewComponentNamespace(@namespace: "CoreWpfTab")]
    public partial class ConfigurationTab : UserControl, IViewComponent
    {
        public ConfigurationTab()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            var logger = scope.Resolve<ILogger<ConfigurationTab>>();
            var wm = scope.Resolve<IWindowManager>();
            if (scope.TryResolve<IConfigurationModelManager>(out var configViewModel))
            {
                this.DataContext = new ConfigurationViewModel(configViewModel, wm);
            }
            else
                logger.LogError("Can't resolve 'IPluginManagerViewModel'");
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            return default;
        }
    }
}
