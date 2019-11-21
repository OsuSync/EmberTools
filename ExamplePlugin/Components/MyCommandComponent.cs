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

namespace ExamplePlugin.Components
{
    public class MyCommandComponent : IComponent, ICommandContainer
    {
        private ILogger<MyCommandComponent> Logger { get; }
        private IPluginsManager PluginsManager { get; }
        private IEventBus EventBus { get; }
        public MyCommandComponent(ILogger<MyCommandComponent> logger, IPluginsManager pluginsManager, IEventBus eventBus)
        {
            Logger = logger;
            PluginsManager = pluginsManager;
            EventBus = eventBus;
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
        }

        [CommandHandler(Command = "event", Parser = typeof(CustomParser))]
        public void MyEventPublisher(int inputNumber)
        {
            EventBus.Publish(new ExamplePluginPublishEvent() { InputNumber = inputNumber });
            Logger.LogInformation($"Event published! Value = {inputNumber}");
        }

        public void Dispose()
        {
        }
    }
}
