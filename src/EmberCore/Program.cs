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
        private static string GetLoggerFilePath(IConfigurationSection config)
        {
            var loggerFolder = config["LogFolder"] ?? "logs";
            var loggerPath = Path.Combine(Directory.GetCurrentDirectory(), loggerFolder);
            if (!Directory.Exists(loggerPath)) Directory.CreateDirectory(loggerPath);
            var loggerFileName = config["LogFilePattern"] ?? "logs_.txt";
            return Path.Combine(loggerPath, loggerFileName);
        }

        private static string GetLoggerConsoleLogFormat(IConfigurationSection config)
        {
            return config["ConsoleLogFormat"]
                ?? "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";
        }
        private static string GetFileLogFormat(IConfigurationSection config)
        {
            return config["FileLogFormat"]
                ?? "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";
        }

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
                    var config = context.Configuration.GetSection("Logging");
                    var loggerFile = GetLoggerFilePath(config);
                    logger
                    .AddConfiguration(config)
                    .AddSerilog((builder) => builder
                        .Enrich.FromLogContext()
                        .WriteTo.Console(
                            theme: SystemConsoleTheme.Colored,
                            outputTemplate: GetLoggerConsoleLogFormat(config))
                        .WriteTo.File(
                            path: loggerFile,
                            rollingInterval: RollingInterval.Day,
                            outputTemplate: GetFileLogFormat(config)))
                    .AddDebug();
                })
                .UseEventBus()
                .UseCommandService()
                .UseKernelService<CorePluginResolver>()
                .UsePlugins<PluginsManager>()
                .UseWindowManager<EmberWpfUIService, Window>()
                .UseMvvmInterface((mvvm) => mvvm
                    .UseConfigurationModel())
                .UseEFSqlite()
                .Build()
                .Run();
        }
    }
}
