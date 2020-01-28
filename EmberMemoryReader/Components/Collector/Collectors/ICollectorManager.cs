using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public interface ICollectorManager
    {
        public Task StartCollectors(CancellationToken token = default);
    }
}
