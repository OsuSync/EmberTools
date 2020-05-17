using Autofac;
using EmberKernel.Services.Command;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
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
using ExamplePlugin.Models.EventSubscription;
using ExamplePlugin.Models;

namespace ExamplePlugin
{

    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin", Version = "1.0")]
    public class MyPlugin : Plugin
    {
        public override Task Initialize(ILifetimeScope scope)
        {
            scope.UseCommandContainer<MyCommandComponent>();
            scope.Subscription<ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
            scope.Subscription<EmptyInfo, MemoryReaderHandler>();
            scope.Subscription<GameStatusInfo, MemoryReaderHandler>();
            return Task.CompletedTask;
        }

        public override Task Uninitialize(ILifetimeScope scope)
        {
            scope.RemoveCommandContainer<MyCommandComponent>();
            scope.Unsubscription<ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
            scope.Unsubscription<EmptyInfo, MemoryReaderHandler>();
            scope.Unsubscription<GameStatusInfo, MemoryReaderHandler>();
            return Task.CompletedTask;
        }

        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.UseConfigurationModel<MyPluginConfiguration>();
            builder.ConfigureCommandContainer<MyCommandComponent>();
            builder.ConfigureStaticEventHandler<MemoryReaderHandler>();
            builder.ConfigureEventHandler<ExamplePluginPublishEvent, ExamplePluginPublishEventHandler>();
        }

    }
}
