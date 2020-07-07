using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Components.Collector
{
    public class CollectorIntervalContainer<T> : ICollectorContainer<T> where T : ICollector
    {
        private ICollector Collector { get; }
        private IEventBus EventBus { get; }
        private ILogger<ICollectorContainer<T>> Logger { get; }
        private bool EventCanCompare { get; }
        private int _retryCount;
        private Func<object, object, bool> EventComparer;
        private object _latestEvent;
        public CollectorIntervalContainer(ILogger<ICollectorContainer<T>> logger, T collector, IEventBus eventBus)
        {
            this.Collector = collector;
            this.EventBus = eventBus;
            this.Logger = logger;
            var comparableType = typeof(IComparableCollector<>);
            var correctType = typeof(T).GetInterface(comparableType.Name);
            if (correctType != null)
            {
                EventCanCompare = true;
                var eventType = correctType.GenericTypeArguments[0];
                EventComparer = (object last, object now) => (bool)eventType.GetMethod("Equals", new[] { eventType }).Invoke(last, new[] { now });
            }
        }

        public async ValueTask Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(Collector.ReadInterval);
                if (!Collector.TryInitialize())
                {
                    Logger.LogDebug($"Memory reader {Collector.GetType().Name} not initialized");
                    continue;
                }

                if (!Collector.TryRead(out Event @event)
                    && ++_retryCount > Collector.RetryLimit)
                {
                    Logger.LogInformation($"Memory reader {Collector.GetType().Name} exceeded max retry limit");
                    break;
                }
                if (!EventCanCompare || _latestEvent == null || !EventComparer(@event, _latestEvent))
                {
                    _latestEvent = @event;
                    EventBus.Publish(@event);
                }
            }
        }
    }
}
