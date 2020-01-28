using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public class CollectorManagerBuilder
    {
        public ContainerBuilder Builder { get; }
        public CollectorManagerBuilder(ContainerBuilder builder)
        {
            this.Builder = builder;
        }

        public CollectorManagerBuilder Data<TCollector, TData>()
            where TCollector : ICollector<TData>
        {
            this.Builder.RegisterType<TCollector>().As<TCollector>();
            return this;
        }

        public CollectorManagerBuilder Configuration(IConfiguration config)
        {
            return this;
        }

        public void Build()
        {
            Builder.RegisterType<CollectorManager>().As<ICollectorManager>().SingleInstance();
        }
    }
}
