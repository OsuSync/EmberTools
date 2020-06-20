using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Util;
using EmberCore.KernelServices.PluginResolver;
using EmberCore.KernelServices.UI.View;
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
        private readonly ILifetimeScope PluginLayerScope;
        private readonly LinkedList<Type> LoadedTypes = new LinkedList<Type>();
        private readonly Dictionary<IPlugin, ILifetimeScope> PluginScopes = new Dictionary<IPlugin, ILifetimeScope>();
        private readonly Dictionary<IPlugin, bool> PluginStatus = new Dictionary<IPlugin, bool>();
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
            var loaders = Resolver.EnumerateLoaders();
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

        public async Task Initialize(IPlugin plugin)
        {
            if (PluginStatus.ContainsKey(plugin) && !PluginStatus[plugin])
            {
                await plugin.Initialize(PluginScopes[plugin]);
                PluginStatus[plugin] = true;
            }
        }

        private async Task InitializeAllPlugins()
        {
            foreach (var (plugin, _) in PluginScopes)
            {
                await Initialize(plugin);
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
                        Logger.LogInformation($"Loaded plugin {pluginDesciptor}");
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Error while load {pluginDesciptor}");
                }
            }
            await InitializeAllPlugins();
        }

        public Task RunEntryComponents()
        {
            Logger.LogInformation($"Start execute entries...");
            Task.WhenAll(EntryComponents.Select(entry => entry.Start()).ToArray())
            .ContinueWith((_) => Logger.LogInformation($"Done execute entries..."));
            return Task.CompletedTask;
        }

        public IEnumerable<Type> Resolve(Assembly assembly)
        {
            foreach (var type in assembly.GetLoadableTypes())
            {
                if (typeof(Plugin).IsAssignableFrom(type))
                {
                    var attribute = type.GetCustomAttribute<EmberPluginAttribute>();
                    if (attribute == null)
                    {
                        Logger.LogError($"Plugin in Assembly {assembly.FullName} no [EmberPluginAttribute] attribute definition. Skipped");
                        continue;
                    }

                    yield return type;
                }
            }
            yield break;
        }

        public Task Load(IPlugin plugin)
        {
            var pluginDesciptor = plugin.GetType().GetCustomAttribute<EmberPluginAttribute>().ToString();
            try
            {
                if (plugin is IEntryComponent entry) EntryComponents.Add(entry);
                var pluginScope = PluginLayerScope.BeginLifetimeScope(plugin.GetType(), (builder) => plugin.BuildComponents(new ComponentBuilder(builder, PluginLayerScope)));
                if (!PluginScopes.ContainsKey(plugin))
                {
                    PluginScopes.Add(plugin, pluginScope);
                    PluginStatus.Add(plugin, false);
                    foreach (var entryRegistrion in pluginScope.ComponentRegistry.Registrations)
                    {
                        if (entryRegistrion.Activator.LimitType.IsAssignableTo<IEntryComponent>())
                        {
                            EntryComponents.Add((IEntryComponent)pluginScope.Resolve(entryRegistrion.Activator.LimitType));
                        }
                    }
                }
                else
                {
                    PluginScopes[plugin] = pluginScope;
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, $"Can't load {pluginDesciptor}");
            }
            return Task.CompletedTask;
        }

        public async Task Unload(IPlugin plugin)
        {
            var pluginDesciptor = plugin.GetType().GetCustomAttribute<EmberPluginAttribute>().ToString();
            Logger.LogInformation($"Unloading plugin {pluginDesciptor}...");
            if (PluginScopes.TryGetValue(plugin, out var scope) && scope != null)
            {
                await plugin.Uninitialize(scope);
                scope.Dispose();
                PluginScopes[plugin] = null;
                PluginStatus[plugin] = false;
                Logger.LogInformation($"Unloaded pluging {pluginDesciptor}!");
            }
            else
            {
                Logger.LogInformation($"Plugin {pluginDesciptor} not initialized");
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
