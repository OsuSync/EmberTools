using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public interface IPluginsLoader : IScopeBuilder
    {
        ValueTask RunEntryComponents();
    }
}
