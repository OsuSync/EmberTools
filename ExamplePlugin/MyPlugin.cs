using Autofac;
using EmberCore.KernelServices.Command;
using EmberCore.KernelServices.Command.Attributes;
using EmberCore.KernelServices.Command.Components;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using ExamplePlugin.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExamplePlugin
{
    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin", Version = "1.0")]
    public class MyPlugin : Plugin, ICommandContainer
    {
        private ICommandService CommandService { get; }
        private ILogger<MyPlugin> Logger { get; }
        private IPluginsManager PluginsManager { get; }
        public MyPlugin(ICommandService commandService, ILogger<MyPlugin> logger, IPluginsManager pluginsManager)
        {
            CommandService = commandService;
            Logger = logger;
            PluginsManager = pluginsManager;
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

        public override Task Initialize(ILifetimeScope scope)
        {
            CommandService.ReigsterCommandContainer(this);
            return Task.CompletedTask;
        }

        public override async Task Uninitialize()
        {
            await Task.Yield();
        }

        public override void BuildComponents(IComponentBuilder builder)
        {

        }

    }
}
