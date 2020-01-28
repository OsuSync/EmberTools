using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors
{
    public static class CollectorBuildHelper
    {
        public static ContainerBuilder Collects(this ContainerBuilder builder, Action<CollectorManagerBuilder> collectorBuilder)
        {
            collectorBuilder(new CollectorManagerBuilder(builder));
            return builder;
        }
    }
}
