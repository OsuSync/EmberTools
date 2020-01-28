using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public class CollectorManager : ICollectorManager
    {
        public ILifetimeScope CurrentScope { get; set; }
        public CollectorManager(ILifetimeScope scope)
        {
            CurrentScope = scope;
        }
        public Task StartCollectors(CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
