using Autofac;
using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Logging;

namespace EmberKernel.Services.Statistic.DataSource.SourceManager
{
    public class EventSourceManager
    {
        private IEventBus EventBus { get; }
        private ILogger<EventSourceManager> Logger { get; }
        public EventSourceManager(IEventBus eventBus, ILogger<EventSourceManager> logger)
        {
            EventBus = eventBus;
            Logger = logger;
        }

        public void Track<TEvent>(ILifetimeScope scope) where TEvent : Event<TEvent>
        {
            Logger.LogInformation($"Start tracking event {typeof(TEvent).Name}");
            EventBus.Subscribe<TEvent, EventWrapperHandler<TEvent>>(scope);
        }

        public void Untrack<TEvent>() where TEvent : Event<TEvent>
        {
            EventBus.Unsubscribe<TEvent, EventWrapperHandler<TEvent>>();
        }
    }
}
