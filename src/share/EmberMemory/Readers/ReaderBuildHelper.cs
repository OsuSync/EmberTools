using Autofac;
using EmberMemory.Components.Collector;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Readers
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
