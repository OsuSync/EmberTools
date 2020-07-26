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

        public static IEnumerable<Variable> CompareAllProperties(T newValue, T oldValue)
        {
            // has value
            foreach (var property in PropertyInfos)
            {
                var newPropertyValue = property.GetValue(newValue);
                if (!Equals(property.GetValue(oldValue), property.GetValue(newValue)))
                {
                    var variable = Variable.CreateFrom(property);
                    variable.Value = Variable.ConvertValue(newPropertyValue);
                    yield return variable;
                }
            }
        }

        private static IEnumerable<Variable> EnumerableAllProperties()
        {
            foreach (var property in PropertyInfos)
            {
                yield return Variable.CreateFrom(property);
            }
            yield break;
        }

        private static IEnumerable<Variable> CompareProperties(T @event)
        {
            var oldValue = Value;
            Value = @event;
            // if current value is 'default'
            if (Equals(Value, DefaultValue))
            {
                // and if incoming event is default, do nothing
                if (Equals(@event, DefaultValue)) return Enumerable.Empty<Variable>();
                // else we return all properties
                else return EnumerableAllProperties();
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
