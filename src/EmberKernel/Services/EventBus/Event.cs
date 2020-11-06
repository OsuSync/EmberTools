using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.EventBus
{
    public abstract class Event
    {
        public string EventName { get; protected set; }
        public Event(string eventName)
        {
            EventName = eventName;
        }
    }

    public abstract class Event<TEvent> : Event where TEvent : Event<TEvent>
    {
        public Event() : base(typeof(TEvent).Name)
        {

        }
    }

    public static class EventTypeExtensions
    {
        public static bool TryReadEventNamespaceAttribute(this Type type, out string @namespace)
        {
            var namespaceAttribute = type.GetCustomAttribute<EventNamespaceAttribute>(false);
            if (namespaceAttribute != null)
            {
                @namespace = namespaceAttribute.Namespace;
                return true;
            }
            @namespace = default;
            return false;
        }

        public static string GetFullEventName(this Type type)
        {
            if (TryReadEventNamespaceAttribute(type, out string @namespace))
            {
                return $"{@namespace}:{type.Name}";
            }
            return type.Name;
        }
    }
}
