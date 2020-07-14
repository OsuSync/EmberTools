using EmberCore.KernelServices.PluginResolver;
using EmberCore.KernelServices.UI.View;
using EmberCore.Services;
using EmberCore.Utils;
using EmberKernel;
using EmberKernel.Services.UI.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.IO;
using System.Windows;

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
                .UsePluginOptions(Path.Combine(Directory.GetCurrentDirectory(), "CoreAppSetting.json"))
                .UseConfigurationModel<CoreAppSetting>()
                .UseLogger((context, logger) =>
                {
                    var configSection = context.Configuration.GetSection("Logging");
                    var loggerFolder = configSection["LogFolder"] ?? "logs";
                    var loggerPath = Path.Combine(Directory.GetCurrentDirectory(), loggerFolder);
                    if (!Directory.Exists(loggerPath)) Directory.CreateDirectory(loggerPath);
                    var loggerFileName = configSection["LogFilePattern"] ?? "logs_.txt";
                    var loggerFile = Path.Combine(loggerPath, loggerFileName);
                    logger
                    .AddConfiguration(configSection)
                    .AddSerilog((builder) => builder
                        .Enrich.FromLogContext()
                        .WriteTo.Console(
                            theme: SystemConsoleTheme.Colored,
                            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                        .WriteTo.File(loggerFile, rollingInterval: RollingInterval.Day))
                    .AddDebug();
                })
                .UseEventBus()
                .UseCommandService()
                .UseKernelService<CorePluginResolver>()
                .UsePlugins<PluginsManager>()
                .UseWindowManager<EmberWpfUIService, Window>()
                .UseMvvmInterface((mvvm) => mvvm
                    .UseConfigurationModel())
                .UseEFSqlite(Path.Combine(Directory.GetCurrentDirectory(), "ember.sqlite"))
                .Build()
                .Run();
        }
    }
}
