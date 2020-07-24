using System;
using Autofac;
using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.DataSource;
using EmberKernel.Services.Statistic.Formatter.DefaultImpl;

namespace EmberKernel
{
    public static class StatisticKernelBuilderExtension
    {
        public static KernelBuilder UseStatistic(this KernelBuilder builder, Action<StatisticBuilder> build)
        {
            builder.Configure((context, container) =>
            {
                container.RegisterType<DefaultFormatter>().As<IFormatter>().SingleInstance();
                container.RegisterType<EmberDataSource>().As<IDataSource>().SingleInstance();
            });
            var statisticBuilder = new StatisticBuilder(builder);
            build(statisticBuilder);
            return builder;
        }
    }
}
