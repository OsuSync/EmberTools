using Autofac;
using EmberMemory.Components.Collector;

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
