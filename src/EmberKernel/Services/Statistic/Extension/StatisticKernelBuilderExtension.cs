using EmberKernel.Services.Statistic;
using System;

namespace EmberKernel
{
    public static class StatisticKernelBuilderExtension
    {
        public static KernelBuilder UseStatistic(this KernelBuilder builder, Action<StatisticBuilder> build)
        {
            var statisticBuilder = new StatisticBuilder(builder);
            build(statisticBuilder);
            return builder;
        }
    }
}
