using Autofac;
using EmberCore.KernelServices.UI.View;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
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
    partial class Main : Window, IHostedWindow
    {
        public Main()
        {
            InitializeComponent();
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            var tabs = scope.Resolve<RegisteredTabs>();
            (FindName("Tabs") as TabControl).ItemsSource = tabs;
            Show();
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            Hide();
            Close();
            return default;
        }
    }
}
