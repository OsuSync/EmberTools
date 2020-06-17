using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.UI.Mvvm.Dependency
{
    public abstract class DependencySet : INotifyPropertyChanged
    {
        private readonly static Dictionary<string, TypeDependency> DependencyMap = new Dictionary<string, TypeDependency>();
        public static IReadOnlyDictionary<string, TypeDependency> Dependencies => DependencyMap;
        public static TypeDependency GetDependency(Type t) => DependencyMap[t.GetFullEventName()];
        public static TypeDependency GetDependency<T>() => GetDependency(typeof(T));
        protected static void AddDependencyProperty(Type type, PropertyInfo property, string attrName)
        {
            var key = type.GetFullEventName();
            if (!DependencyMap.ContainsKey(key))
            {
                DependencyMap.Add(key, new TypeDependency());
            }

            DependencyMap[key].AddDependencyProperty(property, attrName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
        }

        private Type _holdType;
        public DependencySet(Type holdType)
        {
            _holdType = holdType;
        }
        public TypeDependency Dependency => GetDependency(_holdType);

        public abstract object GetValue(string propertyName, PropertyInfo property);
        public abstract void SetValue(string propertyName, PropertyInfo property, object value);
    }

    public abstract class DependencySet<T> : DependencySet
    {
        static DependencySet()
        {
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var _attr = property.GetCustomAttribute<DependencyPropertyAttribute>();
                if (_attr is DependencyPropertyAttribute attr)
                {
                    AddDependencyProperty(type, property, attr.Name);
                }
            }
        }

        public DependencySet() : base(typeof(T))
        {
        }

        protected abstract F GetValue<F>(string propertyName, PropertyInfo property);
        public F GetValue<F>(Expression<Func<T, F>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (!(selector.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(selector));

            if (!(member.Member is PropertyInfo property) || !GetDependency<T>().TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            return GetValue<F>(GetDependency<T>().TypeDependencies[property], property);
        }
        public override object GetValue(string propertyName, PropertyInfo property) => GetValue<object>(propertyName, property);

        protected abstract void SetValue<F>(string propertyName, PropertyInfo property, F value);
        public void SetValue<F>(Expression<Func<T, F>> selector, F value)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (!(selector.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(selector));

            if (!(member.Member is PropertyInfo property) || !GetDependency<T>().TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            SetValue(GetDependency<T>().TypeDependencies[property], property, value);
        }
        public override void SetValue(string propertyName, PropertyInfo property, object value) => SetValue<object>(propertyName, property, value);
    }
}
