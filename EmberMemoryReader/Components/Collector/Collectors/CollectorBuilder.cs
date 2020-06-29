using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public class CollectorBuilder
    {
        internal ContainerBuilder Builder { get; set; }
        public CollectorBuilder(ContainerBuilder builder)
        {
            this.Builder = builder;
        }
        public bool Build()
        {
            return true;
        }


        public CollectorBuilder UseCollectorManager(Action<CollectorManagerBuilder> collectorBuilder)
        {
            var managerBuilder = new CollectorManagerBuilder(Builder);
            collectorBuilder(managerBuilder);
            Builder.RegisterType<CollectorManager>().As<ICollectorManager>();
            return this;
        }

        public CollectorBuilder UseOsuProcessEvent(OsuProcessMatchedEvent @event)
        {
            Builder.RegisterInstance(@event).SingleInstance();
            Builder.Register((ctx) =>
            {
                var process = Process.GetProcessById(ctx.Resolve<OsuProcessMatchedEvent>().ProcessId);
                if (process == null || process.HasExited) throw new EntryPointNotFoundException();
                return process;
            }).As<Process>();
            return this;
        }
    }
}
