using EmberKernel.Services.Statistic.DataSource;
using EmberKernel.Services.Statistic.DataSource.SourceManager;
using EmberKernel.Services.Statistic.Formatter.DefaultImpl;
using EmberKernel.Services.Statistic.Hub;

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
            Builder.UseKernelService<TFormatter, IFormatter>();
            return this;
        }

        public StatisticBuilder ConfigureHub<TStatisticHub>()
            where TStatisticHub : IStatisticHub
        {
            Builder.UseKernelService<TStatisticHub, IStatisticHub>(autoActive: true);
            return this;
        }

        public StatisticBuilder ConfigureDataSource<TDataSource>()
            where TDataSource : IDataSource
        {
            Builder.UseKernelService<TDataSource, IDataSource>();
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

        public StatisticBuilder ConfigureDefaultHub()
        {
            return ConfigureHub<EmberStatisticHub>();
        }
    }
}
