using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;
using EmberKernel.Services.EventBus.Handlers;

namespace EmberKernel.Services.EventBus.SubscriptionManager
{
    public class EventSubscriptionManager : ISubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly Dictionary<Type, List<Type>> _handlerEventTypes;

        public event EventHandler<string> OnEventRemoved;

        public EventSubscriptionManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes = new List<Type>();
            _handlerEventTypes = new Dictionary<Type, List<Type>>();
        }

        public bool IsEmpty => _handlers.Count == 0;
        public void Clear() => _handlers.Clear();

        private string _getFullEventName(string @namespace, string eventName)
        {
            return $"{@namespace}:{eventName}";
        }

        private void AddSubscription(Type handlerType, string fullEventName, bool isDynamic, ILifetimeScope scope)
        {
            if (!HasSubscriptionForEvent(fullEventName))
            {
                _handlers.Add(fullEventName, new List<SubscriptionInfo>());
            }

            if (_handlers[fullEventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{fullEventName}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[fullEventName].Add(SubscriptionInfo.Dynamic(handlerType, scope));
            }
            else
            {
                _handlers[fullEventName].Add(SubscriptionInfo.Typed(handlerType, scope));
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
            if (!_handlerEventTypes.ContainsKey(typeof(THandler)))
            {
                _handlerEventTypes.Add(typeof(THandler), new List<Type>());
            }
            _handlerEventTypes[typeof(THandler)].Add(typeof(TEvent));
        }

        public string GetEventKey(Type t) => t.GetFullEventName();
        public string GetEventKey<T>() => GetEventKey(typeof(T));
        public Type GetEventTypeByName(Type handler, string fullEventName) => _handlerEventTypes[handler].SingleOrDefault(t => t.GetFullEventName() == fullEventName);

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event
        {
            return GetHandlersForEvent(GetEventKey<T>());
        }
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName, string @namespace) => _handlers[_getFullEventName(@namespace, eventName)];

        public bool HasSubscriptionForEvent<T>() where T : Event
        {
            return HasSubscriptionForEvent(GetEventKey<T>());
        }
        public bool HasSubscriptionForEvent(string eventName) => _handlers.ContainsKey(eventName);
        public bool HasSubscriptionForEvent(string eventName, string @namespace) => _handlers.ContainsKey(_getFullEventName(@namespace, eventName));


        private SubscriptionInfo FindSubscriptionToRemove(string fullEventName, Type handlerType)
        {
            if (!HasSubscriptionForEvent(fullEventName))
            {
                return null;
            }
            return _handlers[fullEventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }
        private SubscriptionInfo FindSubscriptionToRemove<TEvent, THandler>()
             where TEvent : Event
             where THandler : IEventHandler<TEvent>
        {
            var fullEventName = GetEventKey<TEvent>();
            return FindSubscriptionToRemove(fullEventName, typeof(THandler));
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
                _handlerEventTypes.Remove(subsToRemove.HandlerType);
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
