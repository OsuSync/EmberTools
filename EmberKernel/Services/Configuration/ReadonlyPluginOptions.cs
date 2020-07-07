using Autofac;
using Microsoft.Extensions.Options;
using System;

namespace EmberKernel.Services.Configuration
{
    public class ReadOnlyPluginOptions<TOptions> : IReadOnlyPluginOptions<TOptions>
        where TOptions : class, new()
    {
        public const string PLUGIN_NAME = "pluginName";
        private IOptionsFactory<TOptions> Factory { get; }
        private string PluginName { get; }
        public ReadOnlyPluginOptions(ILifetimeScope scope, IOptionsFactory<TOptions> factory)
        {
            if (!scope.TryResolveNamed(PLUGIN_NAME, typeof(string), out var registeredPluginName)
                || !(registeredPluginName is string pluginName))
            {
                throw new NullReferenceException("Component not configure with UseConfigurationModel");
            }
            this.PluginName = pluginName;
            this.Factory = factory;
        }
        public TOptions Create()
        {
            return Factory.Create($"{PluginName}.{typeof(TOptions).Name}");
        }
    }
}
