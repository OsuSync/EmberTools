using Autofac;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EmberKernel.Services.Command.HelpGenerator
{
    public class CommandHelp : ICommandHelp
    {
        private ILogger Logger { get; }
        public Type CommandContainerType { get; }
        public IDictionary<string, CommandInfo> CommandInformation { get; }
        public CommandHelp(ILifetimeScope scope, ICommandContainer container, ICommandContainerManager manager)
        {
            CommandContainerType = container.GetType();
            var containerLoggerType = typeof(ILogger<>).MakeGenericType(CommandContainerType);
            Logger = scope.Resolve(containerLoggerType) as ILogger;

            CommandInformation = new Dictionary<string, CommandInfo>();
            foreach (var (methodInfo, handlerAttr, _) in manager.ResolveHandlers())
            {
                var description = methodInfo.GetCustomAttribute<CommandDescriptionAttribute>()?.Description ?? "-";
                CommandInformation.Add(handlerAttr.Command, new CommandInfo()
                {
                    Name = handlerAttr.Command,
                    Description = description
                });
            }
        }


        private void Printer(string str)
        {
            Logger.LogInformation(str);
        }

        public void HandleHelp(CommandArgument argument)
        {
            PrintCommandContainerHelpInformation();
        }

        public void PrintCommandListInformation()
        {
            foreach (var (key, value) in CommandInformation)
            {
                Printer($"\t{key}\t{value.Description}");
            }
        }

        public string GetCommandContainerName(Type containerType)
        {
            var attr = containerType.GetCustomAttribute<CommandContainerNamespaceAttribute>(false);
            if (attr == null) return containerType.Name;
            return attr.Namespace;
        }

        public void PrintCommandContainerHelpInformation()
        {
            Printer($"{GetCommandContainerName(CommandContainerType)} Commands");
            Printer($"Command list:");
            PrintCommandListInformation();
        }
    }
}
