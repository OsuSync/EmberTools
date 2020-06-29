using Autofac;
using EmberMemoryReader.Components.Collector.Collectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Readers
{
    public static class ReaderBuildHelper
    {
        public static CollectorBuilder ReadMemoryWith<T>
            (this CollectorBuilder builder)
            where T : DirectMemoryReader
        {
            builder.Builder.RegisterType<T>().As<DirectMemoryReader>();
            return builder;
        }
    }
}
