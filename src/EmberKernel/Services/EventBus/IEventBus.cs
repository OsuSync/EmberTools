using Autofac;
using EmberKernel.Services.EventBus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.EventBus
{
    public interface IEventBus
    {
        void Publish(Event @event);
        ValueTask Publish(Event @event, CancellationToken cancellationToken = default);

        void Subscribe<TEvent, THandler>(ILifetimeScope currentScope)
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;

        void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;

    }
}
