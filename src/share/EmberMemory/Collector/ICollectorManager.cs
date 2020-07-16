using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Components.Collector
{
    public interface ICollectorManager
    {
        public ValueTask StartCollectors(CancellationToken token = default);
    }
}
