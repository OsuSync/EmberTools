using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Readers
{
    public static class ReaderBuildHelper
    {
        public static ContainerBuilder ReadMemoryWith<T>
            (this ContainerBuilder builder)
            where T : DirectMemoryReader
        {
            builder.RegisterType<T>().As<DirectMemoryReader>();
            return builder;
        }
    }
}
