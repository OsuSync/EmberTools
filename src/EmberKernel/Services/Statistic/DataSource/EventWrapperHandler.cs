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
        public event Action<T, IEnumerable<Variable>> PropertyChanged;

        private static readonly Dictionary<string, PropertyInfo> PropertyInfos
            = new Dictionary<string, PropertyInfo>();
        private static T Value { get; set; } = default;
        private static readonly T DefaultValue = default;

        public static IEnumerable<Variable> GenrateVariables()
        {
            foreach (var (_, property) in PropertyInfos)
            {
                yield return Variable.CreateFrom(property);
            }
        }

        static bool IsPropertyShouldCompare(PropertyInfo property)
        {
            return PropertyInfos.ContainsKey(property.Name);
        }

        static EventWrapperHandler()
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                if (propertyInfo.GetCustomAttribute<DataSourceVariableAttribute>() is DataSourceVariableAttribute)
                {
                    PropertyInfos.Add(propertyInfo.Name, propertyInfo);
                }
            }
        }

        public static IEnumerable<Variable> CompareAllProperties(T newValue, T oldValue)
        {
            // has value
            foreach (var (_, property) in PropertyInfos)
            {
                if (!IsPropertyShouldCompare(property)) continue;
                var newPropertyValue = property.GetValue(newValue);
                if (!Equals(property.GetValue(oldValue), property.GetValue(newValue)))
                {
                    var variable = Variable.CreateFrom(property);
                    if (Equals(newPropertyValue, default)) variable.Value = default;
                    else variable.Value = Variable.ConvertValue(newPropertyValue);
                    yield return variable;
                }
            }
        }

        private static IEnumerable<Variable> EnumerableAllProperties(T newValue)
        {
            foreach (var (_, property) in PropertyInfos)
            {
                if (!IsPropertyShouldCompare(property)) continue;
                var newPropertyValue = property.GetValue(newValue);
                var variable = Variable.CreateFrom(property);
                if (Equals(newPropertyValue, default)) variable.Value = default;
                else variable.Value = Variable.ConvertValue(newPropertyValue);
                yield return variable;
            }
            yield break;
        }

        private static IEnumerable<Variable> CompareProperties(T @event)
        {
            var oldValue = Value;
            Value = @event;
            // if current value is 'default'
            if (Equals(oldValue, DefaultValue))
            {
                // and if incoming event is default, do nothing
                if (Equals(@event, DefaultValue)) return Enumerable.Empty<Variable>();
                // else we return all properties
                else return EnumerableAllProperties(@event);
            }
            // otherwise compare all properties
            return CompareAllProperties(@event, oldValue);
        }

        public ValueTask Handle(T @event)
        {
            PropertyChanged?.Invoke(@event, CompareProperties(@event));
            return default;
        }
    }
}
