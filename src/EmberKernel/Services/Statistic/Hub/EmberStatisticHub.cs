using Autofac;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EmberKernel.Services.Statistic.Hub
{
    public class EmberStatisticHub : ObservableCollection<HubFormat>, IStatisticHub
    {
        private IDataSource DataSource { get; }
        private IFormatter Formatter { get; }
        private ILifetimeScope Scope { get; }

        public IEnumerable<Variable> Variables => DataSource.Variables;
        private readonly Dictionary<string, HubFormat> RegisteredFormats = new Dictionary<string, HubFormat>();

        public EmberStatisticHub(ILifetimeScope scope, IDataSource dataSource, IFormatter formatter)
        {
            DataSource = dataSource;
            Formatter = formatter;
            Scope = scope;
        }

        public event Action<string, string> OnFormatUpdated;

        public string GetValue(string name)
        {
            if (!RegisteredFormats.ContainsKey(name)) { return string.Empty; }
            return RegisteredFormats[name].Value;
        }

        public void Register(string name, string format)
        {
            var hubFormat = new HubFormat()
            {
                Name = name,
                Format = format,
            };
            Add(hubFormat);
            RegisteredFormats.Add(name, hubFormat);
            if (Formatter.IsRegistered<IStatisticHub>(format))
            {
                Formatter.Register<IStatisticHub>(Scope, format);
            }
        }

        public void Unregister(string name)
        {
            if (!RegisteredFormats.ContainsKey(name)) { return; }
            if (Formatter.IsRegistered<IStatisticHub>(RegisteredFormats[name].Format))
            {
                Formatter.Unregister<IStatisticHub>(RegisteredFormats[name].Format);
            }
            Remove(RegisteredFormats[name]);
            RegisteredFormats.Remove(name);
        }

        public ValueTask FormatUpdated(string format, string value)
        {
            foreach (var item in RegisteredFormats.Where((_format) => _format.Value.Format == format))
            {
                item.Value.Value = value;
            }
            OnFormatUpdated?.Invoke(format, value);
            return default;
        }

        public string Format(string format)
        {
            return Formatter.Format(format);
        }
    }
}
