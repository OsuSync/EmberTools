using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using Statistic.WpfUI.UI.ViewModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Statistic.WpfUI.UI.View
{
    /// <summary>
    /// Interaction logic for StatisticEditorTab.xaml
    /// </summary>
    [ViewComponentNamespace(@namespace: "CoreWpfTab")]
    public partial class StatisticEditorTab : UserControl, IViewComponent
    {
        public StatisticEditorTab()
        {
            InitializeComponent();
        }

        public void Dispose()
        {
        }

        public ValueTask Initialize(ILifetimeScope scope)
        {
            this.DataContext = scope.Resolve<StatisticEditorViewModel>();
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            this.DataContext = null;
            return default;
        }
    }
}
