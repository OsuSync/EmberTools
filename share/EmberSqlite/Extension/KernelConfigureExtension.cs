using Autofac;
using EmberSqlite.Integration;
using System.IO;

namespace EmberKernel
{
    public static class KernelConfigureExtension
    {
        public static KernelBuilder UseEFSqlite(this KernelBuilder kernelBuilder, string path)
        {
            kernelBuilder.Configure((_, builder) => builder.RegisterInstance(new SqliteConfiguration(Path.Combine(path))).SingleInstance());
            return kernelBuilder;
        }
    }
}
