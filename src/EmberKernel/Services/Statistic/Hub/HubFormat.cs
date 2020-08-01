using System.ComponentModel;

namespace EmberKernel.Services.Statistic.Hub
{
    public class HubFormat : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public string Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnValueChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public void OnFormatChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Format)));
        }
        public void OnNameChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
}
