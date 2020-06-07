using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Dependency
{
    public abstract class DependencyObject : INotifyPropertyChanged
    {
        private readonly static Dictionary<PropertyInfo, string> _typeDependencies = new Dictionary<PropertyInfo, string>();
        private readonly static Dictionary<string, PropertyInfo> _nameDependencies = new Dictionary<string, PropertyInfo>();
        protected static void AddDependencyProperty(PropertyInfo propertyInfo, string name)
        {
            _typeDependencies.Add(propertyInfo, name);
            _nameDependencies.Add(name, propertyInfo);
        }
        public static IDictionary<PropertyInfo, string> TypeDependencies => _typeDependencies;
        public static IDictionary<string, PropertyInfo> NameDependencies => _nameDependencies;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
        }

        public abstract object GetValue(string propertyName, PropertyInfo property);
        public abstract void SetValue(string propertyName, PropertyInfo property, object value);
    }

    public abstract class DependencyObject<T> : DependencyObject
    {
        static DependencyObject()
        {
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var _attr = property.GetCustomAttribute<DependencyPropertyAttribute>();
                if (_attr is DependencyPropertyAttribute attr)
                {
                    AddDependencyProperty(property, attr.Name);
                }
            }
        }

        public DependencyObject()
        {
        }

        protected abstract F GetValue<F>(string propertyName, PropertyInfo property);
        public F GetValue<F>(Expression<Func<T, F>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (!(selector.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(selector));

            if (!(member.Member is PropertyInfo property) || !TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            return GetValue<F>(TypeDependencies[property], property);
        }
        public override object GetValue(string propertyName, PropertyInfo property) => GetValue<object>(propertyName, property);

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
        public override void SetValue(string propertyName, PropertyInfo property, object value) => SetValue<object>(propertyName, property, value);
    }
}
