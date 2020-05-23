using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Autofac;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.EventBus.SubscriptionManager;
using Microsoft.Extensions.Logging;

namespace EmberKernel.Services.EventBus
{
    public class MemoryEventBus : KernelService, IEventBus
    {
        private readonly ISubscriptionManager _subsManager;

        public MemoryEventBus()
        {
            _subsManager = new EventSubscriptionManager();
        }

        public void Publish(Event @event)
        {
            var eventName = @event.GetType().GetFullEventName();
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            Task.Run(() => ProcessEvent(eventName, jsonMessage));
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (!_subsManager.HasSubscriptionForEvent(eventName)) return;

            var subscriptions = _subsManager.GetHandlersForEvent(eventName);

            foreach (var subscription in subscriptions)
            {
                var scope = subscription.Scope;
                var handler = scope.ResolveOptional(subscription.HandlerType);
                if (handler == null) continue;
                var eventType = _subsManager.GetEventTypeByName(eventName);
                var @event = JsonSerializer.Deserialize(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }
        }

        public void Subscribe<TEvent, THandler>(ILifetimeScope currentScope)
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            _subsManager.AddSubscription<TEvent, THandler>(currentScope);
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            _subsManager.RemoveSubscription<TEvent, THandler>();
        }
    }
}
