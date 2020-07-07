using EmberKernel.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public interface IPluginsManager : IPluginsLoader
    {
        IEnumerable<Type> Resolve(Assembly assembly);
        ValueTask Load(IPlugin plugin);
        ValueTask Initialize(IPlugin plugin);
        ValueTask Unload(IPlugin plugin);

        IPlugin GetPluginByDescriptor(PluginDescriptor descriptor);
        bool IsPluginInitialized(IPlugin plugin);

        IEnumerable<PluginDescriptor> LoadedPlugins();

        IDisposable OnPluginLoad(Action<PluginDescriptor> callback);
        IDisposable OnPluginUnload(Action<PluginDescriptor> callback);
        IDisposable OnPluginInitialized(Action<PluginDescriptor> callback);
    }
}
