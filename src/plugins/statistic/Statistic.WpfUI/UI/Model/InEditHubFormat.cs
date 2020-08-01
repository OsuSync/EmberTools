using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.Hub;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Statistic.WpfUI.UI.Model
{
    public class InEditHubFormat : INotifyPropertyChanged
    {
        private IStatisticHub Hub { get; }
        public InEditHubFormat(IStatisticHub hub)
        {
            Hub = hub;
        }
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        private string _format;
        public string Format
        {
            get => _format;
            set
            {
                if (_format == value) return;
                _format = value;
                OnPropertyChanged();

                Value = Hub.Format(value);
                OnPropertyChanged(nameof(Value));
            }
        }
        public string Value { get; set; }

        public bool IsCreated { get; set; }
        public HubFormat Original { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
