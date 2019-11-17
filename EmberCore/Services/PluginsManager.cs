using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using EmberCore.KernelServices.PluginResolver;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Logging;

namespace EmberCore.Services
{
    internal class PluginsManager : IPluginsManager, IDisposable
    {

        private CorePluginResolver Resolver { get; }
        private ILifetimeScope PluginLayerScope;
        private readonly LinkedList<Type> LoadedTypes = new LinkedList<Type>();
        private readonly Dictionary<IPlugin, ILifetimeScope> PluginScopes = new Dictionary<IPlugin, ILifetimeScope>();
        private ILogger Logger { get; }
        public PluginsManager(CorePluginResolver resolver, ILogger logger)
        {
            Resolver = resolver;
            Logger = logger;
        }

        public void BuildScope(ContainerBuilder builder)
        {
            var loaders = Resolver.EnumerableLoaders();
            foreach (var loader in loaders)
            {
                var assemblies = loader.LoadAssemblies();
                foreach (var assembly in assemblies)
                {
                    foreach (var type in Resolve(assembly))
                    {
                        var attribute = type.GetCustomAttribute<EmberPluginAttribute>();
                        builder.RegisterType(type).Named(attribute.ToString(), type);

                        LoadedTypes.AddLast(type);
                    }
                }
            }
        }

        public void Run(ILifetimeScope scope)
        {
            PluginLayerScope = scope;
            Logger.LogInformation("Start loading plugins scopes...");
            foreach (var type in LoadedTypes)
            {
                var pluginDesciptor = type.GetCustomAttribute<EmberPluginAttribute>().ToString();
                Logger.LogInformation($"Preparing plugin {pluginDesciptor}...");
                var plugin = scope.ResolveNamed(pluginDesciptor, type) as IPlugin;
                Load(plugin);
            }
        }

        public IEnumerable<Type> Resolve(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(Plugin).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<EmberPluginAttribute>();
                    if (attribute == null)
                    {
                        Logger.LogError($"Plugin in Assembly {assembly.FullName} no [CorePlugin] attribute definition. Skipped");
                        continue;
                    }

                    yield return type;
                }
            }
            yield break;
        }

        public void Load(IPlugin plugin)
        {
            var pluginDesciptor = plugin.GetType().GetCustomAttribute<EmberPluginAttribute>().ToString();
            Logger.LogInformation($"Loading plugin {pluginDesciptor}...");
            var pluginScope = PluginLayerScope.BeginLifetimeScope((builder) => plugin.BuildComponents(new ComponentBuilder(builder)));
            PluginScopes.Add(plugin, pluginScope);
            plugin.Initialize(pluginScope);
            Logger.LogInformation($"Loaded pluging {pluginDesciptor}!");
        }

        public void Unload(IPlugin plugin)
        {
            var pluginDesciptor = plugin.GetType().GetCustomAttribute<EmberPluginAttribute>().ToString();
            Logger.LogInformation($"Unloading plugin {pluginDesciptor}...");
            if (PluginScopes.TryGetValue(plugin, out var scope))
            {
                scope.Dispose();
                Logger.LogInformation($"Unloaded pluging {pluginDesciptor}!");
            }
        }

        public void Dispose()
        {
            PluginLayerScope?.Dispose();
        }
    }
}
