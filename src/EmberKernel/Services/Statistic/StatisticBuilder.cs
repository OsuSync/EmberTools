using Autofac;
using EmberKernel.Services.Statistic.DataSource;
using EmberKernel.Services.Statistic.DataSource.SourceManager;
using EmberKernel.Services.Statistic.Formatter;

namespace EmberKernel.Services.Statistic
{
    public class StatisticBuilder
    {
        public KernelBuilder Builder { get; set; }
        public StatisticBuilder(KernelBuilder builder)
        {
            Builder = builder;
        }

        public StatisticBuilder ConfigureEventSourceManager()
        {
            Builder.Configure((context, container) =>
            {
                container.RegisterType<EventSourceManager>().SingleInstance();
            });
            return this;
        }

        public StatisticBuilder ConfigureFormatter<TFormatter>()
            where TFormatter : IFormatter
        {
            Builder.Configure((context, container) =>
            {
                container.RegisterType<TFormatter>().As<IFormatter>().SingleInstance();
            });
            return this;
        }

        public StatisticBuilder ConfigureDataSource<TDataSource>()
            where TDataSource : IDataSource
        {
            Builder.Configure((context, container) =>
            {
                container.RegisterType<TDataSource>().As<IDataSource>().SingleInstance();
            });
            return this;
        }

        public StatisticBuilder ConfigureDefaultDataSource()
        {
            return ConfigureDataSource<EmberDataSource>();
        }

        public StatisticBuilder ConfigureDefaultFormatter()
        {
            return ConfigureFormatter<EmberFormatter>();
        }
    }
}
