using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Components.Collector
{
    public abstract class MemoryDataCollector<TCollector, TMatchedEvent, TTerminatedEvent> : IComponent
        where TCollector : MemoryDataCollector<TCollector, TMatchedEvent, TTerminatedEvent>
        where TMatchedEvent : ProcessMatchedEvent<TMatchedEvent>
        where TTerminatedEvent : ProcessTerminatedEvent<TTerminatedEvent>
    {
        public abstract class Handler<T> : IEventHandler<T> where T : Event<T>
        {
            protected TCollector DataCollector { get; }
            public Handler(TCollector collector)
            {
                this.DataCollector = collector;
            }

            public ValueTask Handle(T @event)
            => @event switch
            {
                TMatchedEvent matched => DataCollector.Handle(matched),
                TTerminatedEvent terminated => DataCollector.Handle(terminated),
                _ => throw new InvalidCastException(),
            };
        }
        public class MatchedHandler : Handler<TMatchedEvent>
        {
            public MatchedHandler(TCollector collector) : base(collector) { }
        }
        public class TerminatedHandler : Handler<TTerminatedEvent>
        {
            public TerminatedHandler(TCollector collector) : base(collector) { }
        }
        private ILifetimeScope CurrentScope { get; set; }
        private ILifetimeScope ManagerScope { get; set; }
        public MemoryDataCollector(ILifetimeScope scope)
        {
            this.CurrentScope = scope;
        }

        private CancellationTokenSource tokenSource;
        public virtual ValueTask Handle(TMatchedEvent @event)
        {
            try
            {
                tokenSource?.Cancel();
                tokenSource?.Dispose();
            } catch { }
            tokenSource = new CancellationTokenSource();
            return StartCollectorAsync(@event);
        }

        public virtual ValueTask Handle(TTerminatedEvent @event)
        {
            try
            {
                using (tokenSource) tokenSource.Cancel();
            }
            catch { }
            return default;
        }
        protected abstract bool BuildCollectScope(CollectorBuilder builder, TMatchedEvent @event);
        public async ValueTask StartCollectorAsync(TMatchedEvent @event)
        {
            var process = Process.GetProcessById(@event.ProcessId);
            if (process == null) return;

            this.ManagerScope = CurrentScope.BeginLifetimeScope((builder) => this.BuildCollectScope(new CollectorBuilder(builder), @event));
            var manager = ManagerScope.Resolve<ICollectorManager>();
            await manager.StartCollectors(tokenSource.Token);
        }

        public void Dispose() 
        {
            tokenSource?.Cancel();
        }
    }
}
