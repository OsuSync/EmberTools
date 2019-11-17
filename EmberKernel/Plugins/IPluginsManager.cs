using EmberKernel.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmberKernel.Plugins
{
    public interface IPluginsManager : IPluginsLoader
    {
        IEnumerable<Type> Resolve(Assembly assembly);
        void Load(IPlugin plugin);
        void Unload(IPlugin plugin);
    }
}
