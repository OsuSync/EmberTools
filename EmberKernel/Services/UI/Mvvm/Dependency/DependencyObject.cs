using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Dependency
{
    public abstract class DependencyObject<T> : INotifyPropertyChanged
    {
        protected readonly static IReadOnlyDictionary<PropertyInfo, string> TypeDependencies;
        static DependencyObject()
        {
            var buildDependencies = new Dictionary<PropertyInfo, string>();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var _attr = property.GetCustomAttribute<DependencyPropertyAttribute>();
                if (_attr is DependencyPropertyAttribute attr)
                {
                    buildDependencies.Add(property, attr.Name);
                }
            }
            TypeDependencies = buildDependencies;
        }

        public DependencyObject()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
        }

        protected abstract F GetValue<F>(string propertyName, Func<T, F> selector);
        public F GetValue<F>(Expression<Func<T, F>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (!(selector.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(selector));

            if (!(member.Member is PropertyInfo property) || !TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            return GetValue(TypeDependencies[property], selector.Compile());
        }

        protected abstract void SetValue<F>(string propertyName, PropertyInfo property, F value);
        public void SetValue<F>(Expression<Func<T, F>> selector, F value)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (!(selector.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(selector));

            if (!(member.Member is PropertyInfo property) || !TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            SetValue(TypeDependencies[property], property, value);
        }

    }
}
