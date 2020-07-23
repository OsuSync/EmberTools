using Autofac;
using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.DataSource;

namespace EmberKernel
{
    public static class StatisticKernelBuilderExtension
    {
        public static KernelBuilder UseStatisticDataSource(this KernelBuilder builder)
        {
            builder.Configure((context, container) =>
            {
                container.RegisterType<EmberDataSource>().As<IDataSource>().SingleInstance();
            });
            return builder;
        }
    }
}
