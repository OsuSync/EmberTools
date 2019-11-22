using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.EventBus;
using ExamplePlugin.Commands;
using ExamplePlugin.Models.EventPublisher;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin.Components
{
    class PluginControlCommand : IComponent, ICommandContainer
    {
        private readonly IPluginsManager _pluginMan;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PluginControlCommand> _logger;

        public PluginControlCommand(IPluginsManager pluginMan, IEventBus eventBus, ILogger<PluginControlCommand> logger)
        {
            _pluginMan = pluginMan;
            _eventBus = eventBus;
            _logger = logger;
        }

        [CommandHandler(Command = "enable")]
        public void EnablePlugin(string plugin)
        {
            _pluginMan.Load(FindPlugin(plugin));
        }

        [CommandHandler(Command = "disable")]
        public void DisablePlugin(string plugin)
        {
            _pluginMan.Unload(FindPlugin(plugin));
        }

        [CommandHandler(Command = "event", Parser = typeof(CustomParser))]
        public void MyEventPublisher(int inputNumber)
        {
            _eventBus.Publish(new ExamplePluginPublishEvent() { InputNumber = inputNumber });
            _logger.LogInformation($"Event published! Value = {inputNumber}");
        }

        public IPlugin FindPlugin(string plugin)
        {
            foreach (var descriptor in _pluginMan.LoadedPlugins())
            {
                if (descriptor.Name == plugin)
                {
                    return _pluginMan.GetPluginByDescriptor(descriptor);
                }
            }
            return null;
        }

        public void Dispose()
        {
        }
    }
}
