using Autofac;
using EmberSqlite.Integration;
using System.IO;

namespace EmberKernel
{
    public static class KernelConfigureExtension
    {
        public static KernelBuilder UseEFSqlite(this KernelBuilder kernelBuilder)
        {
            kernelBuilder.Configure((context, builder) =>
            {
                var dbFileName = context.Configuration.GetSection("Sqlite")["DbFileName"] ?? "ember.sqlite";
                builder.RegisterInstance(new SqliteConfiguration(Path.GetFullPath(dbFileName))).SingleInstance();
            });
            return kernelBuilder;
        }
    }
}
