using System.Collections.Generic;
using System.Reflection;

namespace EmberKernel.Services.UI.Mvvm.Dependency
{
    public class TypeDependency
    {
        private readonly Dictionary<PropertyInfo, string> _typeDependencies = new Dictionary<PropertyInfo, string>();
        private readonly Dictionary<string, PropertyInfo> _nameDependencies = new Dictionary<string, PropertyInfo>();

        public void AddDependencyProperty(PropertyInfo propertyInfo, string name)
        {
            _typeDependencies.Add(propertyInfo, name);
            _nameDependencies.Add(name, propertyInfo);
        }
        public IReadOnlyDictionary<PropertyInfo, string> TypeDependencies => _typeDependencies;
        public IReadOnlyDictionary<string, PropertyInfo> NameDependencies => _nameDependencies;
    }
}
