using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EmberCore.KernelServices.PluginResolver;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using Microsoft.Extensions.Logging;

namespace EmberCore.Services
{
    internal class PluginsManager : IPluginsManager, IDisposable
    {

        private CorePluginResolver Resolver { get; }
        private ILifetimeScope PluginLayerScope;
        private readonly LinkedList<Type> LoadedTypes = new LinkedList<Type>();
        private readonly Dictionary<IPlugin, ILifetimeScope> PluginScopes = new Dictionary<IPlugin, ILifetimeScope>();
        private readonly List<IEntryComponent> EntryComponents = new List<IEntryComponent>();
        private ILogger<PluginsManager> Logger { get; }
        public PluginsManager(ILifetimeScope scope, CorePluginResolver resolver, ILogger<PluginsManager> logger)
        {
            Resolver = resolver;
            Logger = logger;
            PluginLayerScope = scope;
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
                        var pluginDesciptor = type.GetCustomAttribute<EmberPluginAttribute>().ToString();
                        builder.RegisterType(type).Named(pluginDesciptor, type);

                        LoadedTypes.AddLast(type);
                    }
                }
            }
        }

        public async Task Run(ILifetimeScope scope)
        {
            foreach (var type in LoadedTypes)
            {
                var pluginDesciptor = type.GetCustomAttribute<EmberPluginAttribute>().ToString();
                Logger.LogInformation($"Preparing plugin {pluginDesciptor}...");
                try
                {
                    Logger.LogInformation($"Resolving plugin {pluginDesciptor}...");
                    if (scope.TryResolveNamed(pluginDesciptor, type, out var instnace) && instnace is IPlugin plugin)
                    {
                        await Load(plugin);
                        if (plugin is IEntryComponent entry) EntryComponents.Add(entry);
                        Logger.LogInformation($"Loaded plugin {pluginDesciptor}");
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error while load {pluginDesciptor}");
                }
            }
        }

        public async Task RunEntryComponents()
        {
            Logger.LogInformation($"Start execute entries...");
            await Task.WhenAll(EntryComponents.Select(entry => entry.Start()).ToArray());
            Logger.LogInformation($"Done execute entries...");
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

        public async Task Load(IPlugin plugin)
        {
            try
            {
                var pluginDesciptor = plugin.GetType().GetCustomAttribute<EmberPluginAttribute>().ToString();
                var pluginScope = PluginLayerScope.BeginLifetimeScope((builder) => plugin.BuildComponents(new ComponentBuilder(builder)));
                if (!PluginScopes.ContainsKey(plugin))
                {
                    PluginScopes.Add(plugin, pluginScope);
                }
                else
                {
                    PluginScopes[plugin] = pluginScope;
                }
                await plugin.Initialize(pluginScope);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        public async Task Unload(IPlugin plugin)
        {
            var pluginDesciptor = plugin.GetType().GetCustomAttribute<EmberPluginAttribute>().ToString();
            Logger.LogInformation($"Unloading plugin {pluginDesciptor}...");
            if (PluginScopes.TryGetValue(plugin, out var scope))
            {
                await plugin.Uninitialize(scope);
                scope.Dispose();
                PluginScopes[plugin] = null;
                Logger.LogInformation($"Unloaded pluging {pluginDesciptor}!");
            }
        }

        public void Dispose()
        {
            PluginLayerScope?.Dispose();
        }

        public IEnumerable<PluginDescriptor> LoadedPlugins()
        {
            foreach (var plugin in PluginScopes.Keys)
            {
                yield return PluginDescriptor.FromAttribute(plugin.GetType().GetCustomAttribute<EmberPluginAttribute>());
            }
        }

        public IPlugin GetPluginByDescriptor(PluginDescriptor descriptor)
        {
            foreach (var plugin in PluginScopes.Keys)
            {
                var pluginDescriptor = PluginDescriptor.FromAttribute(plugin.GetType().GetCustomAttribute<EmberPluginAttribute>());
                if (pluginDescriptor.ToString() == descriptor.ToString())
                {
                    return plugin;
                }
            }
            return null;
        }
    }
}
