using EmberCore.KernelServices.UI.View.Configuration.Components;
using EmberKernel.Plugins;
using EmberKernel.Services.UI.Mvvm.Dependency;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Controls;

namespace EmberCore.KernelServices.UI.ViewModel.Configuration
{
    public class WpfFeatureBuilder
    {
        public const string WpfRenderControl = "WpfRenderControl";
        public const string WpfExtraData = "WpfExtraData";
        public const string WpfMultiOptionFields = "WpfRenderViewType";
    }
    public class WpfFeatureBuilder<TPlugin, TOptions> : WpfFeatureBuilder
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        protected readonly DependencySet dependencySet;

        public WpfFeatureBuilder(DependencySet dependencySet)
        {
            this.dependencySet = dependencySet;
        }

        public PropertyInfo GetSelectProperty<TPropertyType>(Expression<Func<TOptions, TPropertyType>> expression)
        {
            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException("Invalid lambda for binding", nameof(expression));

            if (!(member.Member is PropertyInfo property) || !DependencySet.GetDependency<TOptions>().TypeDependencies.ContainsKey(property))
                throw new ArgumentException("Not a property or property not registered", member.Member.Name);

            return property;
        }

        public T GetOrCreateDependencyAttach<T>(string key)
            where T : new()
        {
            if (!dependencySet.HasAttach(key))
            {
                dependencySet.Attach(key, new T());
            }
            return dependencySet.GetAttach<T>(key);
        }

        public WpfFeatureBuilder<TPlugin, TOptions> UseControl<TPropertyType>(Expression<Func<TOptions, TPropertyType>> selector, Type type, object extraData = null)
        {
            var controls = GetOrCreateDependencyAttach<Dictionary<string, Type>>(WpfRenderControl);
            var propertyName = GetSelectProperty(selector).Name;
            controls.Add(propertyName, type);
            if (extraData != null)
            {
                var extra = GetOrCreateDependencyAttach<Dictionary<string, object>>(WpfExtraData);
                extra.Add(propertyName, extraData);
            }
            return this;
        }
        public WpfFeatureBuilder<TPlugin, TOptions> UseControl<TControl, TProperty>(Expression<Func<TOptions, TProperty>> selector, object extraData = null)
            where TControl : Control
            => UseControl(selector, typeof(TControl), extraData);

        public WpfFeatureBuilder<TPlugin, TOptions> UseComboList<TPropertyType, TViewModel>(
            Expression<Func<TOptions, TPropertyType>> selector,
            TViewModel collections)
            where TViewModel : INotifyCollectionChanged
        {
            var renderType = GetOrCreateDependencyAttach<HashSet<string>>(WpfMultiOptionFields);
            var propertyName = GetSelectProperty(selector).Name;
            renderType.Add(propertyName);
            return UseControl(selector, typeof(ComboComponent), collections);
        }
    }
}
