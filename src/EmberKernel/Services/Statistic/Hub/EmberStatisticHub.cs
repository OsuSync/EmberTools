using Autofac;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        public HubFormat this[string name] => RegisteredFormats[name];

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
            RegisteredFormats.Add(name, hubFormat);
            if (!Formatter.IsRegistered<IStatisticHub>(format))
            {
                Formatter.Register<IStatisticHub>(Scope, name, format);
            }
            Add(hubFormat);
        }

        public void Unregister(string name)
        {
            if (!RegisteredFormats.ContainsKey(name)) { return; }
            if (Formatter.IsRegistered<IStatisticHub>(name))
            {
                Formatter.Unregister<IStatisticHub>(name);
            }
            Remove(RegisteredFormats[name]);
            RegisteredFormats.Remove(name);
        }

        public ValueTask FormatUpdated(string format, string value)
        {
            foreach (var item in RegisteredFormats.Where((_format) => _format.Value.Format == format))
            {
                item.Value.Value = value;
                item.Value.OnValueChanged();
            }
            OnFormatUpdated?.Invoke(format, value);
            return default;
        }

        public string Format(string format)
        {
            return Formatter.Format(format);
        }

        public void Update(string name, string format, string newName = null)
        {
            if (!RegisteredFormats.ContainsKey(name)) { return; }
            var shouldUpdateName = (newName ?? name) != name;
            if (shouldUpdateName)
            {
                if (RegisteredFormats.ContainsKey(newName))
                {
                    throw new DuplicateNameException();
                }
                RegisteredFormats.Remove(name);
                RegisteredFormats.Add(newName, RegisteredFormats[name]);
                if (Formatter.IsRegistered<IStatisticHub>(name))
                {
                    Formatter.Unregister<IStatisticHub>(name);
                }
                if (Formatter.IsRegistered<IStatisticHub>(newName))
                {
                    Formatter.Unregister<IStatisticHub>(newName);
                }
                Formatter.Register<IStatisticHub>(Scope, newName, format);
            }
            var operatorName = newName ?? name;
            if (Formatter.IsRegistered<IStatisticHub>(operatorName))
            {
                Formatter.Update<IStatisticHub>(operatorName, format);
            }
            RegisteredFormats[operatorName].Format = format;
            RegisteredFormats[operatorName].OnFormatChanged();
            RegisteredFormats[operatorName].Name = operatorName;
            if (shouldUpdateName)
            {
                RegisteredFormats[operatorName].OnNameChanged();
            }
            RegisteredFormats[operatorName].Value = Formatter.Format(format);
            RegisteredFormats[operatorName].OnValueChanged();

        }

        public bool IsRegistered(string name) => RegisteredFormats.ContainsKey(name);
    }
}
