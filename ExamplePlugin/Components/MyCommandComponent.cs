using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using ExamplePlugin.Commands;
using ExamplePlugin.Models;
using ExamplePlugin.Models.EventPublisher;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using EmberKernel.Services.Command;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace ExamplePlugin.Components
{
    public class MyCommandComponent : IComponent, ICommandContainer
    {
        private ILogger<MyCommandComponent> Logger { get; }
        private IPluginsManager PluginsManager { get; }
        private IOptionsSnapshot<MyPluginConfiguration> Config { get; }

        public MyCommandComponent(ILogger<MyCommandComponent> logger, IPluginsManager pluginsManager, IOptionsSnapshot<MyPluginConfiguration> config)
        {
            Logger = logger;
            PluginsManager = pluginsManager;
            Config = config;
        }

        [CommandHandler(Command = "my", Parser = typeof(CustomParser))]
        public void MyCommand(int args)
        {
            Logger.LogInformation($"My command invoked, args + 1 = {args + 1}");
            Logger.LogInformation($"Installed plugins: ");
            foreach (var descriptor in PluginsManager.LoadedPlugins())
            {
                Logger.LogInformation($"{descriptor.Name} by {descriptor.Author} ver {descriptor.Version}");
            }
            Logger.LogInformation($"Config Value Config.Value.MyIntValue = {Config.Value.MyIntValue}");
        }

        public void Dispose()
        {
        }
    }
}
