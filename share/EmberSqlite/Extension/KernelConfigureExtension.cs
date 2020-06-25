using Autofac;
using EmberKernel;
using EmberSqlite.Integration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel
{
    public static class KernelConfigureExtension
    {
        public static KernelBuilder UseEFSqlite(this KernelBuilder kernelBuilder, string path = "ember.db")
        {
            kernelBuilder.Configure((_, builder) => builder.RegisterInstance(new SqliteConfiguration(path)).SingleInstance());
            return kernelBuilder;
        }
    }
}
