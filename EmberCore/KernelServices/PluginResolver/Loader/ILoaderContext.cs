using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmberCore.KernelServices.PluginResolver.Loader
{
    public interface ILoaderContext
    {
        string CurrentPath { get; }
        IEnumerable<Assembly> LoadAssemblies();
    }
}
