using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public static class CollectorBuildHelper
    {
        public static ContainerBuilder UseCollectorManager(this ContainerBuilder builder, Action<CollectorManagerBuilder> collectorBuilder)
        {
            var managerBuilder = new CollectorManagerBuilder(builder);
            collectorBuilder(managerBuilder);
            builder.RegisterType<CollectorManager>().As<ICollectorManager>();
            return builder;
        }

        public static ContainerBuilder UseOsuProcessEvent(this ContainerBuilder builder, OsuProcessMatchedEvent @event)
        {
            builder.RegisterInstance(@event);
            builder.Register((ctx) =>
            {
                var process = Process.GetProcessById(ctx.Resolve<OsuProcessMatchedEvent>().ProcessId);
                if (process == null || process.HasExited) throw new EntryPointNotFoundException();
                return process;
            }).As<Process>();
            return builder;
        }
    }
}
