using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using EmberWpfCore.Components.Configuration.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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
            if (scope.TryResolve<IConfigurationModelManager>(out var configViewModel))
            {
                this.DataContext = new ConfigurationViewModel(configViewModel);
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
