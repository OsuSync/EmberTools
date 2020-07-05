using Autofac;
using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Components.Collector
{
    public class CollectorManager : ICollectorManager, IDisposable
    {
        public ILifetimeScope CurrentScope { get; set; }
        public ILifetimeScope CollectorReadScope { get; set; }
        private HashSet<Type> RegisteredCollector { get; }
        private ILogger<CollectorManager> Logger { get; }
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        public CollectorManager(ILifetimeScope scope, ILogger<CollectorManager> logger)
        {
            Logger = logger;
            CurrentScope = scope;
            RegisteredCollector = scope.ResolveNamed<HashSet<Type>>(CollectorManagerBuilder.RegisteredTypesType);
        }

        public ValueTask StartCollectors(CancellationToken token = default)
        {
            var genericContianerType = typeof(CollectorIntervalContainer<>);
            var genericContianerIType = typeof(CollectorIntervalContainer<>);
            // Filter all instance which can resolve as ICollector
            CollectorReadScope = CurrentScope.BeginLifetimeScope((builder) =>
            {
                foreach (var type in RegisteredCollector)
                {
                    var instance = CurrentScope.Resolve(type);
                    if (instance is ICollector collector)
                    {
                        var containerType = genericContianerType.MakeGenericType(type);
                        var containerIType = genericContianerIType.MakeGenericType(type);
                        builder.RegisterType(containerType).As(containerIType);
                    }
                }
            });

            foreach (var type in RegisteredCollector)
            {
                var containerIType = genericContianerIType.MakeGenericType(type);
                var container = CollectorReadScope.Resolve(containerIType) as ICollectorContainer;
                container.Run(token);
            }
            return default;
        }

        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
