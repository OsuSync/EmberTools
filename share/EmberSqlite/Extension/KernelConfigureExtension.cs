using Autofac;
using EmberKernel;
using EmberSqlite.Integration;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
