using EmberKernel.Plugins;
using EmberKernel.Plugins.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public interface IPluginsManager : IPluginsLoader
    {
        IEnumerable<Type> Resolve(Assembly assembly);
        Task Load(IPlugin plugin);
        Task Initialize(IPlugin plugin);
        Task Unload(IPlugin plugin);

        IPlugin GetPluginByDescriptor(PluginDescriptor descriptor);

        IEnumerable<PluginDescriptor> LoadedPlugins();
    }
}
