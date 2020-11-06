using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Plugins.Components
{
    public interface IEntryComponent : IComponent
    {
        ValueTask Start();
    }
}
