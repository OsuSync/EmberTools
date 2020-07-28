using EmberKernel.Services.Statistic.DataSource;
using EmberKernel.Services.Statistic.DataSource.SourceManager;
using EmberKernel.Services.Statistic.Formatter.DefaultImpl;

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
            Builder.UseKernelService<EventSourceManager, IEventSourceManager>();
            return this;
        }

        public StatisticBuilder ConfigureFormatter<TFormatter>()
            where TFormatter : IFormatter
        {
            Builder.UseKernelService<DefaultFormatter, IFormatter>();
            return this;
        }

        public StatisticBuilder ConfigureDataSource<TDataSource>()
            where TDataSource : IDataSource
        {
            Builder.UseKernelService<EmberDataSource, IDataSource>();
            return this;
        }

        public StatisticBuilder ConfigureDefaultDataSource()
        {
            return ConfigureDataSource<EmberDataSource>();
        }

        public StatisticBuilder ConfigureDefaultFormatter()
        {
            return ConfigureFormatter<DefaultFormatter>();
        }
    }
}
