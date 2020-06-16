using Autofac;
using EmberCore.KernelServices.UI.View;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewModel.Configuration;
using EmberWpfCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EmberWpfCore.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    partial class Main : Window, IHostedWpfWindow
    {
        public static IConfigurationModelManager ConfigurationManager { get; set; }
        public Main()
        {
            InitializeComponent();
        }
        ConfigurationViewModel ViewMode { get; set; }
        public void Initialize(ILifetimeScope scope)
        {
            var manager = scope.Resolve<IConfigurationModelManager>();
            ConfigurationManager = manager;
            ViewMode = new ConfigurationViewModel(manager.GetDependency("MyPluginConfiguration"));
            this.DataContext = ViewMode;
            if (this.FindName("configurations") is ListBox list)
            {
                list.ItemsSource = manager;
            }
        }
    }
}
