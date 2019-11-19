using Autofac;
using EmberCore.KernelServices.Command;
using EmberCore.KernelServices.Command.Attributes;
using EmberCore.KernelServices.Command.Components;
using EmberCore.KernelServices.Command.Models;
using EmberCore.KernelServices.Command.Parsers;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExamplePlugin
{
    [EmberPlugin(Author = "ZeroAsh", Name = "ExamplePlugin", Version = "1.0")]
    public class MyPlugin : Plugin, ICommandContainer
    {
        private ICommandServices CommandService { get; }
        private ILogger<MyPlugin> Logger { get; }
        public MyPlugin(ICommandServices commandService, ILogger<MyPlugin> logger)
        {
            CommandService = commandService;
            Logger = logger;
        }

        private class CustomParser : IParser
        {
            public IEnumerable<object> ParseCommandArgument(CommandArgument args)
            {
                if (int.TryParse(args.Argument, out var parsedInt)) yield return parsedInt;
                else yield return 0;
            }
        }

        [CommandHandler(Command = "my", Parser = typeof(CustomParser))]
        public void MyCommand(int args)
        {
            Logger.LogInformation($"My command invoked, args + 1 = {args + 1}");
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
