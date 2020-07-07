using System.Collections.Generic;
using System.Reflection;

namespace EmberCore.KernelServices.PluginResolver.Loader
{
    public interface ILoaderContext
    {
        string CurrentPath { get; }
        IEnumerable<Assembly> LoadAssemblies();
    }
}
