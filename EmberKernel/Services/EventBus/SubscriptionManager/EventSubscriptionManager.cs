using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EmberKernel.Services.EventBus.Handlers;

namespace EmberKernel.Services.EventBus.SubscriptionManager
{
    public class EventSubscriptionManager : ISubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public EventSubscriptionManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty => _handlers.Count == 0;
        public void Clear() => _handlers.Clear();

        private void AddSubscription(Type handlerType, string eventName, bool isDynamic, ILifetimeScope scope)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType, scope));
            }
            else
            {
                _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType, scope));
            }
        }
        public void AddSubscription<TEvent, THandler>(ILifetimeScope scope)
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            AddSubscription(typeof(THandler), eventName, isDynamic: false, scope);

            if (!_eventTypes.Contains(typeof(TEvent)))
            {
                _eventTypes.Add(typeof(TEvent));
            }
        }

        public string GetEventKey<T>() => typeof(T).Name;
        public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event
        {
            return GetHandlersForEvent(GetEventKey<T>());
        }
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public bool HasSubscriptionForEvent<T>() where T : Event
        {
            return HasSubscriptionForEvent(GetEventKey<T>());
        }
        public bool HasSubscriptionForEvent(string eventName) => _handlers.ContainsKey(eventName);


        private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                return null;
            }
            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }
        private SubscriptionInfo FindSubscriptionToRemove<TEvent, THandler>()
             where TEvent : Event
             where THandler : IEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            return FindSubscriptionToRemove(eventName, typeof(THandler));
        }

        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }
        private void RemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers.ContainsKey(eventName))
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }

            }
        }
        public void RemoveSubscription<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            var handlerToRemove = FindSubscriptionToRemove<TEvent, THandler>();
            var eventName = GetEventKey<TEvent>();
            RemoveHandler(eventName, handlerToRemove);
        }
    }
}
