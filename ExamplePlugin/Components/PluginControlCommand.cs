using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Configuration;
using EmberKernel.Services.Command.Models;
using EmberKernel.Services.EventBus;
using ExamplePlugin.Commands;
using ExamplePlugin.Models;
using ExamplePlugin.Models.EventPublisher;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePlugin.Components
{
    [CommandContainerNamespace("plugin")]
    [CommandContainerAlias("p")]
    [CommandContainerAlias(".")]
    class PluginControlCommand : IComponent, ICommandContainer
    {
        private readonly IPluginsManager _pluginMan;
        private readonly IEventBus _eventBus;
        private readonly ILogger<PluginControlCommand> _logger;
        private readonly IReadOnlyPluginOptions<MyPluginConfiguration> _options;

        public PluginControlCommand(IPluginsManager pluginMan, IEventBus eventBus, ILogger<PluginControlCommand> logger, IReadOnlyPluginOptions<MyPluginConfiguration> options)
        {
            _pluginMan = pluginMan;
            _eventBus = eventBus;
            _logger = logger;
            _options = options;
        }

        [CommandHandler(Command = "enable")]
        public async ValueTask EnablePlugin(string plugin)
        {
            var pluginInstance = FindPlugin(plugin);
            if (pluginInstance == null)
            {
                _logger.LogWarning($"Plugin {plugin} not found!");
                return;
            }
            await _pluginMan.Load(pluginInstance);
            await _pluginMan.Initialize(pluginInstance);
        }

        [CommandHandler(Command = "disable")]
        public async ValueTask DisablePlugin(string plugin)
        {
            await _pluginMan.Unload(FindPlugin(plugin));
        }

        [CommandHandler(Command = "event")]
        [CommandParser(typeof(CustomParser))]
        public void MyEventPublisher(int inputNumber)
        {
            _eventBus.Publish(new ExamplePluginPublishEvent() { InputNumber = inputNumber });
            _logger.LogInformation($"Event published! Value = {inputNumber}");
        }

        [CommandHandler(Command = "async-event")]
        [CommandParser(typeof(CustomParser))]
        public void MyAsyncEventPublisher(int inputNumber)
        {
            _eventBus.Publish(new ExamplePluginPublishEvent() { InputNumber = inputNumber }, default).AsTask().Wait();
            _logger.LogInformation($"Event published! Value = {inputNumber}");
        }

        [CommandHandler(Command = "conf")]
        public void ReadPluginConfiguration()
        {
            _logger.LogInformation($"Plugin configuration === {_options.Create().LatestBeatmapFile}");
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

        public bool TryAssignCommand(CommandArgument argument, out CommandArgument newArgument)
        {
            newArgument = argument;
            return false;
        }
    }
}
