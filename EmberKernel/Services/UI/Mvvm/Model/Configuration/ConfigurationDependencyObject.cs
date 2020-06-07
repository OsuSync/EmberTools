﻿using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Services.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.Model.Configuration
{
    public class ConfigurationDependencyObject<TPlugin, TOptions> : DependencyObject<TOptions>
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        private readonly IOptionsMonitor<TOptions> Option;
        private TOptions CurrentValue;
        public ConfigurationDependencyObject(ILifetimeScope scope)
        {
            if (!(scope.Resolve<IOptionsMonitor<TOptions>>() is var option))
            {
                throw new ArgumentNullException(typeof(TOptions).Name, "Option not registered with UsePluginOptionsModel");
            }
            Option = option;
            Option.OnChange((latestOption) =>
            {
                CurrentValue = latestOption;
                foreach (var item in TypeDependencies)
                {
                    RaisePropertyChangedEvent(this, new PropertyChangedEventArgs(item.Value));
                }
            });
            this.CurrentValue = Option.CurrentValue;
        }

        protected override F GetValue<F>(string propertyName, Func<TOptions, F> getter)
        {
            return getter(CurrentValue);
        }

    }
}
