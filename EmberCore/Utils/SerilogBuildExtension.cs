using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace EmberCore.Utils
{
    public static class SerilogBuildExtension
    {
        public static ILoggingBuilder AddSerilog(this ILoggingBuilder builder, Func<LoggerConfiguration, LoggerConfiguration> rootBuilder)
        {
            var conf = new LoggerConfiguration();
            rootBuilder(conf);
            Log.Logger = conf.CreateLogger();

            builder.AddSerilog(logger: Log.Logger, dispose: true);
            return builder;
        }
    }
}
