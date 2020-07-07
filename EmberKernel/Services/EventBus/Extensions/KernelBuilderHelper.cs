using Autofac;
using EmberKernel.Services.EventBus;

namespace EmberKernel
{
    public static class KernelBuilderHelper
    {
        public static KernelBuilder UseEventBus(this KernelBuilder builder)
        {
            builder.Configure((context, container) =>
            {
                container.RegisterType<MemoryEventBus>().As<IEventBus>().SingleInstance();
            });
            return builder;
        }
    }
}
