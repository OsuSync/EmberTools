using Autofac;
using EmberKernel.Services.EventBus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.EventBus.SubscriptionManager
{
    public interface ISubscriptionManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<TEvent, THandler>(ILifetimeScope scope)
           where TEvent : Event
           where THandler : IEventHandler<TEvent>;

        void RemoveSubscription<TEvent, THandler>()
           where TEvent : Event
           where THandler : IEventHandler<TEvent>;

        bool HasSubscriptionForEvent<T>() where T : Event;
        bool HasSubscriptionForEvent(string eventName);
        Type GetEventTypeByName(Type handler, string eventName);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event;
        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);
        string GetEventKey<T>();

    }
}
