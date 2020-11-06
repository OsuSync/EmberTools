using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource;
using EmberKernel.Services.Statistic.DataSource.SourceManager;

namespace EmberKernel.Services.Statistic.Extension
{
    public static class EventSourceManagerExtension
    {
        public static void ConfigureEventStatistic<TEvent>(this IComponentBuilder builder)
            where TEvent : Event<TEvent>
        {
            if (!(builder is ComponentBuilder compBuilder)) return;

            compBuilder.Container.RegisterType<EventWrapperHandler<TEvent>>().SingleInstance();
        }

        public static void Track<TEvent>(this ILifetimeScope scope)
            where TEvent : Event<TEvent>
        {
            if (!(scope.ResolveOptional<IEventSourceManager>() is IEventSourceManager eventSourceManager)) return;
            eventSourceManager.Track<TEvent>(scope);
        }

        public static void Untrack<TEvent>(this ILifetimeScope scope)
            where TEvent : Event<TEvent>
        {
            if (!(scope.ResolveOptional<IEventSourceManager>() is IEventSourceManager eventSourceManager)) return;
            eventSourceManager.Untrack<TEvent>();
        }
    }
}
