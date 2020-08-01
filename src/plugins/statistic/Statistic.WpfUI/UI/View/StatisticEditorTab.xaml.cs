using Autofac;
using EmberKernel.Services.UI.Mvvm.ViewComponent;
using Statistic.WpfUI.UI.Model;
using Statistic.WpfUI.UI.ViewModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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

        private IEditorContextViewModel _viewModel;
        public ValueTask Initialize(ILifetimeScope scope)
        {
            _viewModel = scope.Resolve<IEditorContextViewModel>();
            this.DataContext = _viewModel;
            return default;
        }

        public ValueTask Uninitialize(ILifetimeScope scope)
        {
            this.DataContext = null;
            return default;
        }
        public void OnFormatDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _viewModel.EditingHubFormat = new InEditHubFormat(_viewModel.Formats)
            {
                IsCreated = false,
                Format = _viewModel.SelectedHubFormat.Format,
                Name = _viewModel.SelectedHubFormat.Name,
                Value = _viewModel.SelectedHubFormat.Value,
                Original = _viewModel.SelectedHubFormat,
            };
            _viewModel.Mode = EditorMode.Editing;
        }
    }
}
