using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent.Window;
using EmberWpfCore.ViewModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
