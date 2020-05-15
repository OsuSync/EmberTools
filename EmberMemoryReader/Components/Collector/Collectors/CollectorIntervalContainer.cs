using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public class CollectorIntervalContainer<T> : ICollectorContainer<T> where T : ICollector
    {
        private ICollector Collector { get; }
        private IEventBus EventBus { get; }
        private ILogger<ICollectorContainer<T>> Logger { get; }
        private int _retryCount;
        public CollectorIntervalContainer(ILogger<ICollectorContainer<T>> logger, T collector, IEventBus eventBus)
        {
            this.Collector = collector;
            this.EventBus = eventBus;
            this.Logger = logger;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!Collector.TryRead(out Event @event)
                    && ++_retryCount > Collector.RetryLimit)
                {
                    Logger.LogInformation($"Memory reader {Collector.GetType().Name} exceeded max retry limit");
                    break;
                }
                EventBus.Publish(@event);
                await Task.Delay(Collector.ReadInterval); 
            }
        }
    }
}
