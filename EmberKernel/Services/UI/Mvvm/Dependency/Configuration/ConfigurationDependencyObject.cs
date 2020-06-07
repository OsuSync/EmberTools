using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Services.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.Dependency.Configuration
{
    public class ConfigurationDependencyObject<TPlugin, TOptions> : DependencyObject<TOptions>, IDisposable
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        private readonly IOptionsMonitor<TOptions> Option;
        private readonly IPluginOptions<TPlugin, TOptions> PluginOptions;
        private TOptions CurrentValue;
        private readonly IDisposable OnChangeBinding;
        public ConfigurationDependencyObject(ILifetimeScope scope)
        {
            if (!(scope.Resolve<IOptionsMonitor<TOptions>>() is IOptionsMonitor<TOptions> option)
                || !(scope.Resolve<IPluginOptions<TPlugin, TOptions>>() is IPluginOptions<TPlugin, TOptions> pluginOption))
            {
                throw new ArgumentNullException(typeof(TOptions).Name, "Option not registered with UsePluginOptionsModel");
            }
            Option = option;
            PluginOptions = pluginOption;
            OnChangeBinding = Option.OnChange((latestOption) =>
            {
                CurrentValue = latestOption;
                foreach (var item in TypeDependencies)
                {
                    RaisePropertyChangedEvent(this, new PropertyChangedEventArgs(item.Value));
                }
            });
            this.CurrentValue = Option.CurrentValue;
        }

        public void Dispose()
        {
            OnChangeBinding.Dispose();
        }

        protected override F GetValue<F>(string propertyName, Func<TOptions, F> getter)
        {
            return getter(CurrentValue);
        }

        protected override void SetValue<F>(string propertyName, PropertyInfo property, F value)
        {
            var latestValue = PluginOptions.Create();
            property.SetValue(latestValue, value);
            PluginOptions.SaveAsync(latestValue).Wait();
        }
    }
}
