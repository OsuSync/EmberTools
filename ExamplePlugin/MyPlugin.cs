using Autofac;
using EmberCore.KernelServices.Command;
using EmberCore.KernelServices.Command.Attributes;
using EmberCore.KernelServices.Command.Components;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using ExamplePlugin.Commands;
using ExamplePlugin.Components;
using ExamplePlugin.EventHandlers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExamplePlugin
{

    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin", Version = "1.0")]
    public class MyPlugin : Plugin
    {
        private ICommandService CommandService { get; }
        public MyPlugin(ICommandService commandService)
        {
            CommandService = commandService;
        }

        public override Task Initialize(ILifetimeScope scope)
        {
            CommandService.ReigsterCommandContainer(scope.Resolve<MyCommandComponent>());
            scope.Subscription<Models.EventSubscription.ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
            return Task.CompletedTask;
        }

        public override async Task Uninitialize()
        {
            await Task.Yield();
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<MyCommandComponent>();
            builder.ConfigureEventHandler<Models.EventSubscription.ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
        }

    }
}
