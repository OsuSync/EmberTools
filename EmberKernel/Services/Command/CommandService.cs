using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Builder;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.Models;
using EmberKernel.Services.Command.Parsers;
using EmberKernel.Services.Command.Sources;
using EmberKernel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmberKernel.Services.Command
{
    public class CommandService : KernelService, ICommandService
    {
        private ILogger<CommandService> Logger { get; }
        private int CommandSourceOperationTimeLimit { get; }
        private ILifetimeScope ParentScope { get; }
        private ILifetimeScope CurrentCommandScope { get; set; }
        private CancellationTokenSource BackgroundCancellationSource { get; set; }
        private Task CurrentRunningCommandLooping { get; set; }
        public CommandService(IConfiguration coreAppSetting, ILogger<CommandService> logger, ILifetimeScope scope)
        {
            Logger = logger;
            CommandSourceOperationTimeLimit = int.Parse(coreAppSetting["CommandSourceOperationTimeLimit"] ?? "5");
            ParentScope = scope;
            ConfigureCommandSource(builder => builder.ConfigureSource<ConsoleSource>()).Wait();
            Keeper = new Thread(() =>
            {
                while (notDisposed) Thread.Sleep(1);
            });
            Keeper.Start();
        }

        private class CommandHandlerInfo
        {
            public ICommandContainer CommandHandlerComponent { get; set; }
            public MethodInfo CommandHandler { get; set; }
            public Type HandlerParserType { get; set; }
        }
        private readonly Dictionary<ICommandContainer, LinkedList<string>> componentCommands = new Dictionary<ICommandContainer, LinkedList<string>>();
        private readonly Dictionary<string, CommandHandlerInfo> commandHandlers = new Dictionary<string, CommandHandlerInfo>();
        private readonly Dictionary<Type, IParser> parsers = new Dictionary<Type, IParser>()
        {
            { typeof(DefaultParser), new DefaultParser() },
        };
        private bool notDisposed = true;
        private readonly Thread Keeper;

        private IEnumerable<(MethodInfo, CommandHandlerAttribute)> ResolveHandlers(ICommandContainer component)
        {
            var type = component.GetType();
            foreach (var method in type.GetMethods())
            {
                if (method.GetCustomAttribute<CommandHandlerAttribute>() is CommandHandlerAttribute attr)
                {
                    yield return (method, attr);
                }
            }
        }

        public void ReigsterCommandContainer(ICommandContainer commandComponent)
        {
            foreach(var (method, attr) in ResolveHandlers(commandComponent))
            {
                // Get a safe command name to take command duplicate situation
                var safeCommandName = attr.Command;
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
                    CommandHandlerComponent = commandComponent,
                    CommandHandler = method,
                };

                // Add parser instance to dictionary
                // Check parser assignable
                if (!typeof(IParser).IsAssignableFrom(attr.Parser))
                {
                    Logger.LogWarning($"Command handler parser type not implement IParse interface!");
                }
                // Add 'commandHandler' as Parser
                if (attr.Parser == commandComponent.GetType())
                {
                    parsers.Add(attr.Parser, commandComponent as IParser);
                }
                // Create new parser instance in Arrtibute
                else if (attr.Parser != typeof(DefaultParser) && !parsers.ContainsKey(attr.Parser))
                {
                    parsers.Add(attr.Parser, Activator.CreateInstance(attr.Parser) as IParser);
                }
                // Associate command and parser
                commandHandlerInfo.HandlerParserType = attr.Parser;
                if (!componentCommands.ContainsKey(commandComponent)) componentCommands.Add(commandComponent, new LinkedList<string>());
                componentCommands[commandComponent].AddLast(safeCommandName);
                commandHandlers.Add(safeCommandName, commandHandlerInfo);
            }
        }
        public void RemoveHandler(ICommandContainer commandComponent)
        {
            if (!componentCommands.ContainsKey(commandComponent)) return;

            foreach (var command in componentCommands[commandComponent])
            {
                commandHandlers.Remove(command);
            }
            componentCommands.Remove(commandComponent);
        }

        public void Dispose()
        {
            notDisposed = false;
            Keeper.Abort();
            StopCurrentCommandLooping().Wait();
            commandHandlers.Clear();
            componentCommands.Clear();
            parsers.Clear();
            if (CurrentCommandScope != null)
            {
                CurrentCommandScope.Dispose();
            }
        }

        public async Task StopCurrentCommandLooping()
        {
            if (BackgroundCancellationSource != null && !BackgroundCancellationSource.IsCancellationRequested) BackgroundCancellationSource.Cancel();
            if (CurrentRunningCommandLooping != null) await CurrentRunningCommandLooping;
            if (CurrentCommandScope != null) using (CurrentCommandScope) { }
        }

        public async Task ConfigureCommandSource(Action<ICommandSourceBuilder> builder)
        {
            // Clean and dispose previous ICommandSource
            await StopCurrentCommandLooping();

            // Build and start new ICommandSource lifecycles
            CurrentCommandScope = ParentScope.BeginLifetimeScope((container) => builder(new CommandSourceBuilder(container)));

            BackgroundCancellationSource = new CancellationTokenSource();
            CurrentRunningCommandLooping = Task.Run(() => BackgroundLooping(BackgroundCancellationSource.Token));
        }

        private CancellationTokenSource CreateOperationTimeLimitSource()
            => new CancellationTokenSource(TimeSpan.FromSeconds(CommandSourceOperationTimeLimit));

        private async Task BackgroundLooping(CancellationToken token)
        {
            // Resolve ICommandSource
            using var commandSource = CurrentCommandScope.Resolve<ICommandSource>();
            if (commandSource == null) return;

            // Do initilization action
            using var initCancellationSource = CreateOperationTimeLimitSource();
            await commandSource.Initialize(initCancellationSource.Token);

            // Looping
            while (!token.IsCancellationRequested) DispatchCommand(await commandSource.Read(default));

            // Do stop action
            using var stopCancellationSource = CreateOperationTimeLimitSource();
            await commandSource.Stop(stopCancellationSource.Token);
        }

        private void DispatchCommand(CommandArgument argument)
        {
            if (!commandHandlers.ContainsKey(argument.Command))
            {
                Logger.LogError($"Can't dispatch unknown command {argument.Command}.");
                return;
            }
            var handlerInfo = commandHandlers[argument.Command];
            var parser = parsers[handlerInfo.HandlerParserType];
            handlerInfo.CommandHandler.Invoke(handlerInfo.CommandHandlerComponent, parser.ParseCommandArgument(argument).ToArray());
        }
    }
}
