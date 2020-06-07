using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Model
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

        protected abstract F GetValue<F>(string propertyName, Func<T, F> getter);
        public F GetValue<F>(Expression<Func<T, F>> getter)
        {
            if (getter == null) throw new ArgumentNullException(nameof(getter));
            if (!(getter.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(getter));

            if (!(member.Member is PropertyInfo property) || !TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            return GetValue(TypeDependencies[property], getter.Compile());
        }
    }
}
