using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.Hub;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IComponent = EmberKernel.Plugins.Components.IComponent;

namespace Statistic.WpfUI.UI.ViewModel
{
    public class StatisticEditorViewModel : IComponent, INotifyPropertyChanged
    {
        public IStatisticHub Formats { get; set; }
        public IDataSource Variables { get; set; }

        private HubFormat _selectedHubFormat;

        public event PropertyChangedEventHandler PropertyChanged;

        public HubFormat SelectedHubFormat
        {
            get => _selectedHubFormat;
            set
            {
                if (Equals(_selectedHubFormat, value)) return;
                _selectedHubFormat = value;
                OnPropertyChanged();
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose() { }
    }
}
