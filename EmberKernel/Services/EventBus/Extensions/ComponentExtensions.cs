using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public static class ComponentExtensions
    {
        public static void ConfigureEventHandler<TEvent, THandler>(this IComponentBuilder builder)
            where THandler : IEventHandler<TEvent>
        {
            if (!(builder is ComponentBuilder compBuilder)) return;

            compBuilder.Container.RegisterType<THandler>();
        }

        public static void ConfigureStaticEventHandler<TEvent, THandler>(this IComponentBuilder builder)
            where THandler : IEventHandler<TEvent>
        {
            if (!(builder is ComponentBuilder compBuilder)) return;

            compBuilder.Container.RegisterType<THandler>().SingleInstance();
        }

        public static void Subscription<TEvent, THandler>(this ILifetimeScope scope)
            where THandler : IEventHandler<TEvent>
            where TEvent : Event
        {
            if (!(scope.ResolveOptional<IEventBus>() is IEventBus eventBus)) return;
            eventBus.Subscribe<TEvent, THandler>(scope);
        }

        public static void Unsubscription<TEvent, THandler>(this ILifetimeScope scope)
            where THandler : IEventHandler<TEvent>
            where TEvent : Event
        {
            if (!(scope.ResolveOptional<IEventBus>() is IEventBus eventBus)) return;
            eventBus.Unsubscribe<TEvent, THandler>();
        }
    }
}
