using Autofac;
using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.DataSource;
using EmberKernel.Services.Statistic.Formatter;

namespace EmberKernel
{
    public static class StatisticKernelBuilderExtension
    {
        public static KernelBuilder UseStatistic(this KernelBuilder builder)
        {
            builder.Configure((context, container) =>
            {
                container.RegisterType<EmberFormatter>().As<IFormatter>().SingleInstance();
                container.RegisterType<EmberDataSource>().As<IDataSource>().SingleInstance();
            });
            return builder;
        }
    }
}
