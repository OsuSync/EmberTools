using Autofac;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource.Variables;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.DataSource.SourceManager
{
    public class EventSourceManager : IEventSourceManager
    {
        private IEventBus EventBus { get; }
        private ILogger<EventSourceManager> Logger { get; }
        private IDataSource DataSource { get; }
        public EventSourceManager(IEventBus eventBus, ILogger<EventSourceManager> logger, IDataSource dataSource)
        {
            EventBus = eventBus;
            Logger = logger;
            DataSource = dataSource;
        }

        public void Track<TEvent>(ILifetimeScope scope) where TEvent : Event<TEvent>
        {
            Logger.LogInformation($"Start tracking event {typeof(TEvent).Name}");
            var wrapper = scope.ResolveOptional<EventWrapperHandler<TEvent>>();
            if (wrapper == null)
            {
                Logger.LogWarning($"EventWrapper {typeof(TEvent).Name} not registered, skipped.");
                return;
            }
            foreach (var variable in EventWrapperHandler<TEvent>.GenrateVariables())
            {
                DataSource.Register(variable, variable.Value);
            }
            wrapper.PropertyChanged += this.Wrapper_PropertyChanged;
            EventBus.Subscribe<TEvent, EventWrapperHandler<TEvent>>(scope);
        }

        private void Wrapper_PropertyChanged<TEvent>(TEvent @event, IEnumerable<Variable> changedProperties)
             where TEvent : Event<TEvent>
        {
            DataSource.Publish(changedProperties);
        }

        public void Untrack<TEvent>() where TEvent : Event<TEvent>
        {
            EventBus.Unsubscribe<TEvent, EventWrapperHandler<TEvent>>();
        }
    }
}
