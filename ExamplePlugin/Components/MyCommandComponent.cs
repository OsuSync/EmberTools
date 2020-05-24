﻿using EmberKernel.Services.Command.Attributes;
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
using EmberKernel.Services.Configuration;
using EmberKernel.Services.Command.Models;

namespace ExamplePlugin.Components
{
    [CommandContainerNamespace("my")]
    public class MyCommandComponent : IComponent, ICommandContainer
    {
        private ILogger<MyCommandComponent> Logger { get; }
        private IPluginsManager PluginsManager { get; }
        private IPluginOptions<MyPlugin, MyPluginConfiguration> Config { get; }

        public MyCommandComponent(ILogger<MyCommandComponent> logger, IPluginsManager pluginsManager, IPluginOptions<MyPlugin, MyPluginConfiguration> config)
        {
            Logger = logger;
            PluginsManager = pluginsManager;
            Config = config;
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

        public void Dispose()
        {
        }

        public bool TryAssignCommand(CommandArgument argument, out CommandArgument newArgument)
        {
            newArgument = argument.MoveToHelpCommand();
            return true;
        }
    }
}
