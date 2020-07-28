using Autofac;
using EmberKernel.Services.EventBus;

namespace EmberKernel.Services.Statistic.DataSource.SourceManager
{
    public interface IEventSourceManager : IKernelService
    {
        void Track<TEvent>(ILifetimeScope scope) where TEvent : Event<TEvent>;
        void Untrack<TEvent>() where TEvent : Event<TEvent>;
    }
}
