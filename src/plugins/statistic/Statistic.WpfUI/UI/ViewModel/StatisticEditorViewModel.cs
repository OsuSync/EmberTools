using EmberKernel.Services.Statistic;
using System.ComponentModel;
using IComponent = EmberKernel.Plugins.Components.IComponent;

namespace Statistic.WpfUI.UI.ViewModel
{
    class StatisticEditorViewModel : IComponent, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IStatisticHub Formats { get; set; }
        public IDataSource Variables { get; set; }

        public void Dispose() { }
    }
}
