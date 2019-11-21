using System;
using System.Collections.Generic;
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
}
