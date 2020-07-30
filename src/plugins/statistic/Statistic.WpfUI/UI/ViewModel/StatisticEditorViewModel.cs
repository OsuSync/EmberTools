using EmberKernel.Services.Statistic;
using IComponent = EmberKernel.Plugins.Components.IComponent;

namespace Statistic.WpfUI.UI.ViewModel
{
    class StatisticEditorViewModel : IComponent
    {
        public IStatisticHub Formats { get; set; }
        public IDataSource Variables { get; set; }

        public void Dispose() { }
    }
}
