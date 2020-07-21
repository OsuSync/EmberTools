using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmberKernel.Services.Statistic.DataSource
{
    public class EventWrapperHandler<T> : IEventHandler<T>
    {
        public event Action<IEnumerable<string>> PropertyChanged;

        private static readonly List<PropertyInfo> PropertyInfos
            = new List<PropertyInfo>();
        private static T Value { get; set; } = default;
        private static readonly T DefaultValue = default;

        public static IEnumerable<Variable> GenrateVariables()
        {
            foreach (var property in PropertyInfos)
            {
                yield return Variable.CreateFrom(property);
            }
        }

        static EventWrapperHandler()
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                PropertyInfos.Add(propertyInfo);
            }
        }

        public static IEnumerable<string> CompareAllProperties(T newValue, T oldValue)
        {
            // has value
            foreach (var property in PropertyInfos)
            {
                if (!Equals(property.GetValue(oldValue), property.GetValue(newValue)))
                {
                    yield return property.Name;
                }
            }
        }

        private IEnumerable<string> EnumerableAllProperties()
        {
            foreach (var property in PropertyInfos)
            {
                yield return property.Name;
            }
            yield break;
        }

        private IEnumerable<string> CompareProperties(T @event)
        {
            var oldValue = Value;
            Value = @event;
            // if current value is default
            if (Equals(Value, DefaultValue))
            {
                // and if incoming event is default, do nothing
                if (Equals(@event, DefaultValue)) return Enumerable.Empty<string>();
                // else we return all properties
                else return EnumerableAllProperties();
            }
            // otherwise compare all properties
            return CompareAllProperties(@event, oldValue);
        }

        public ValueTask Handle(T @event)
        {
            PropertyChanged?.Invoke(CompareProperties(@event));
            return default;
        }
    }
}
