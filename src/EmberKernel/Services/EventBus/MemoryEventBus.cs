﻿using Autofac;
using EmberKernel.Services.EventBus.Handlers;
using EmberKernel.Services.EventBus.SubscriptionManager;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.EventBus
{
    public class MemoryEventBus : IKernelService, IEventBus, IDisposable
    {
        private readonly ISubscriptionManager _subsManager;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public MemoryEventBus()
        {
            _subsManager = new EventSubscriptionManager();
        }

        public void Publish(Event @event)
        {
            if (_cancellationTokenSource.IsCancellationRequested) return;
            Publish(@event, _cancellationTokenSource.Token);
        }

        public ValueTask Publish(Event @event, CancellationToken cancellation = default)
        {
            var eventName = @event.GetType().GetFullEventName();
            var jsonMessage = JsonSerializer.Serialize(@event, @event.GetType());
            return ProcessEvent(eventName, jsonMessage, cancellation);
        }

        private async ValueTask ProcessEvent(string eventName, string message, CancellationToken cancellation = default)
        {
            if (!_subsManager.HasSubscriptionForEvent(eventName)) return;

            var subscriptions = _subsManager.GetHandlersForEvent(eventName);

            foreach (var subscription in subscriptions)
            {
                var scope = subscription.Scope;
                var handler = scope.ResolveOptional(subscription.HandlerType);
                if (handler == null) continue;
                var eventType = _subsManager.GetEventTypeByName(subscription.HandlerType, eventName);
                var @event = JsonSerializer.Deserialize(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                var method = concreteType.GetMethod("Handle");
                if (method.GetParameters().Length == 2)
                {
                    await (ValueTask)method.Invoke(handler, new object[] { @event, cancellation });
                }
                else
                {
                    await (ValueTask)method.Invoke(handler, new object[] { @event });
                }
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

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
