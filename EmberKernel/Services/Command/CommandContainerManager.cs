using Autofac;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.HelpGenerator;
using EmberKernel.Services.Command.Models;
using EmberKernel.Services.Command.Parsers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmberKernel.Services.Command
{
    public class CommandContainerManager : ICommandContainerManager
    {
        private class CommandHandlerInfo
        {
            public MethodInfo CommandHandler { get; set; }
            public Type HandlerParserType { get; set; }
            public bool IsAwaitable { get; set; }
        }

        private ILogger<CommandContainerManager> Logger { get; }
        private ICommandContainer CommandContainer { get; }
        private Type CommandContainerType { get; }
        private readonly Dictionary<string, CommandHandlerInfo> commandHandlers = new Dictionary<string, CommandHandlerInfo>();
        private readonly Dictionary<Type, IParser> parsers = new Dictionary<Type, IParser>()
        {
            { typeof(DefaultParser), new DefaultParser() },
        };
        private ILifetimeScope CurrentScope { get; }
        public CommandContainerManager(ILifetimeScope scope, ICommandContainer container, ILogger<CommandContainerManager> logger)
        {
            CommandContainer = container;
            CommandContainerType = container.GetType();
            Logger = logger;
            CurrentScope = scope;
        }

        IEnumerable<(MethodInfo, CommandHandlerAttribute, Type)> ICommandContainerManager.ResolveHandlers() => ResolveHandlers();
        private IEnumerable<(MethodInfo, CommandHandlerAttribute, Type)> ResolveHandlers()
        {
            var type = CommandContainer.GetType();
            foreach (var method in type.GetMethods())
            {
                if (method.GetCustomAttribute<CommandHandlerAttribute>() is CommandHandlerAttribute attr)
                {
                    var parser = method.GetCustomAttribute<CommandParserAttribute>()?.Parser ?? typeof(DefaultParser);
                    yield return (method, attr, parser);
                }
            }
        }

        public void InitializeHandlers(Action<string, CommandHandlerAttribute> globalCommandRegister)
        {
            foreach (var (method, cmdAttr, parserType) in ResolveHandlers())
            {
                // Get a safe command name to take command duplicate situation
                var safeCommandName = cmdAttr.Command;
                var cmdIndex = 0;
                while (commandHandlers.ContainsKey(safeCommandName))
                {
                    safeCommandName = $"{safeCommandName}{++cmdIndex}";
                }
                if (cmdIndex > 0)
                {
                    Logger.LogWarning($"Command {safeCommandName} was duplicated! Rename to '{safeCommandName}'");
                }
                // Add command handlers to dictionary
                var commandHandlerInfo = new CommandHandlerInfo
                {
                    CommandHandler = method,
                    IsAwaitable = method.ReturnType.GetMethods().Any((method) => method.Name == "GetAwaiter"),
                };

                // Add parser instance to dictionary
                // Check parser assignable
                if (!typeof(IParser).IsAssignableFrom(parserType))
                {
                    Logger.LogWarning($"Command handler parser type not implement IParse interface!");
                }
                // Add 'commandHandler' as Parser
                if (parserType == CommandContainerType)
                {
                    parsers.Add(parserType, CommandContainer as IParser);
                }
                // Create new parser instance in Arrtibute
                else if (parserType != typeof(DefaultParser) && !parsers.ContainsKey(parserType))
                {
                    parsers.Add(parserType, Activator.CreateInstance(parserType) as IParser);
                }

                // Associate command and parser
                commandHandlerInfo.HandlerParserType = parserType;
                commandHandlers.Add(safeCommandName, commandHandlerInfo);
                
                // if command has CommandAlias attribute, register as global command
                if (method.GetCustomAttribute<CommandAliasAttribute>() is CommandAliasAttribute attr)
                {
                    globalCommandRegister(attr.Alias, cmdAttr);
                }
            }
        }

        public void RemoveHandlers()
        {
            foreach (var command in commandHandlers.Keys)
            {
                commandHandlers.Remove(command);
            }
        }

        public void Dispose()
        {
            commandHandlers.Clear();
            parsers.Clear();
        }

        public async ValueTask Invoke(CommandArgument argument)
        {
            var currentArgument = argument;
            if (argument.Command == null)
            {
                if (!CommandContainer.TryAssignCommand(argument, out var nextArgument))
                {
                    Logger.LogWarning($"Unknown command {argument.Command}");
                    return;
                }
                else currentArgument = nextArgument;
            }

            if (currentArgument.Command == "help" && CurrentScope.TryResolve<ICommandHelp>(out var help))
            {
                help.HandleHelp(argument);
                return;
            }

            if (!commandHandlers.TryGetValue(currentArgument.Command, out var handlerInfo) || handlerInfo == null)
            {
                Logger.LogWarning($"Unknown command {argument.Command}");
                return;
            }
            object ret;
            if (handlerInfo.CommandHandler.GetParameters().Length == 0)
            {
                ret = handlerInfo.CommandHandler.Invoke(CommandContainer, null);
            }
            else
            {
                var parser = parsers[handlerInfo.HandlerParserType];
                ret = handlerInfo.CommandHandler.Invoke(CommandContainer, parser.ParseCommandArgument(argument).ToArray());
            }
            if (ret != null && handlerInfo.IsAwaitable)
            {
                switch (ret)
                {
                    case Task task:
                        await task;
                        return;
                    case ValueTask valueTask:
                        await valueTask;
                        return;
                }
            }
        }
    }
}
