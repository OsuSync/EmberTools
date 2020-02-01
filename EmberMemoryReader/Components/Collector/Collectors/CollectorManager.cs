using Autofac;
using EmberKernel.Services.EventBus;
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
        private HashSet<Type> RegisteredCollector { get; }
        private readonly LinkedList<ICollector> Collectors = new LinkedList<ICollector>();
        public CollectorManager(ILifetimeScope scope)
        {
            CurrentScope = scope;
            RegisteredCollector = scope.ResolveNamed<HashSet<Type>>(CollectorManagerBuilder.RegisteredTypesType);
        }
        public Task StartCollectors(CancellationToken token = default)
        {
            // Filter all instance which can resolve as ICollector
            foreach (var type in RegisteredCollector)
            {
                var instance = CurrentScope.Resolve(type);
                if (instance is ICollector collector)
                {
                    Collectors.AddLast(collector);
                }
            }
            throw new NotImplementedException();
        }
    }
}
