using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemory.Components.Collector
{
    public class CollectorBuilder
    {
        public ContainerBuilder Builder { get; set; }
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
    }
}
