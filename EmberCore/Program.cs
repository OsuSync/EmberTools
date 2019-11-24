using EmberKernel.Services.Command;
using EmberCore.KernelServices.PluginResolver;
using EmberCore.Services;
using EmberKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EmberCore
{
    class Program
    {
        public static void Main()
        {
            new KernelBuilder()
                .UseConfiguration((config) =>
                {
                    config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddEnvironmentVariables()
                    .AddJsonFile("CoreAppSetting.json", optional: false, reloadOnChange: true);
                })
                .UseConfigurationModel<CoreAppSetting>()
                .UseLogger((context, logger) =>
                {
                    logger
                    .AddConfiguration(context.Configuration.GetSection("Logging"))
                    .AddConsole(options => options.Format = ConsoleLoggerFormat.Systemd)
                    .AddDebug();
                })
                .UseKernalService<CorePluginResolver>()
                .UsePlugins<PluginsManager>()
                .UseCommandService()
                .UseEventBus()
                .Build()
                .Run();
        }
    }
}
