using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.Models;
using EmberKernel.Services.Configuration;
using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.Format;
using ExamplePlugin.Commands;
using ExamplePlugin.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExamplePlugin.Components
{
    [CommandContainerNamespace("my")]
    public class MyCommandComponent : IComponent, ICommandContainer, IFormatContainer
    {
        private ILogger<MyCommandComponent> Logger { get; }
        private IPluginsManager PluginsManager { get; }
        private IPluginOptions<MyPlugin, MyPluginConfiguration> Config { get; }
        private IFormatter Formatter { get; }
        private IDataSource DataSource { get; }
        private ILifetimeScope Scope { get; }

        public MyCommandComponent(
            ILifetimeScope scope,
            ILogger<MyCommandComponent> logger,
            IPluginsManager pluginsManager,
            IPluginOptions<MyPlugin, MyPluginConfiguration> config,
            IFormatter formatter,
            IDataSource dataSource)
        {
            Scope = scope;
            Logger = logger;
            PluginsManager = pluginsManager;
            Config = config;
            Formatter = formatter;
            DataSource = dataSource;
        }

        [CommandHandler(Command = "my")]
        [CommandParser(typeof(CustomParser))]
        public void MyCommand(int args)
        {
            Logger.LogInformation($"My command invoked, args + 1 = {args + 1}");
            Logger.LogInformation($"Installed plugins: ");
            foreach (var descriptor in PluginsManager.LoadedPlugins())
            {
                Logger.LogInformation($"{descriptor.Name} by {descriptor.Author} ver {descriptor.Version}");
            }
            Logger.LogInformation($"Config Value Config.Value.MyIntValue = {Config.Create().MyIntValue}");
        }

        [CommandHandler(Command = "last")]
        [CommandAlias("l")]
        public void LatestBeatmapFile()
        {
            Logger.LogInformation($"Latest beatmap file= {Config.Create().LatestBeatmapFile}");
        }

        [CommandHandler(Command = "var")]
        [CommandAlias("var")]
        public void Variables()
        {
            foreach (var item in DataSource.Variables)
            {
                Logger.LogInformation($"[{item.Id}]Name={item.Name},Value={item.Value.GetString()}");
            }
        }

        [CommandHandler(Command = "format")]
        [CommandAlias("format")]
        public void Format(string format)
        {
            Formatter.Register<MyCommandComponent>(Scope, format);
            Logger.LogInformation($"Format Registered:{format}");
        }

        public void Dispose()
        {
        }

        public bool TryAssignCommand(CommandArgument argument, out CommandArgument newArgument)
        {
            newArgument = argument.MoveToHelpCommand();
            return true;
        }

        public ValueTask FormatUpdated(string format, string value)
        {
            Logger.LogInformation($"Format:{format},Result:{value}");
            return default;
        }
    }
}
