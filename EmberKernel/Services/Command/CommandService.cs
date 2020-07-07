using Autofac;
using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Builder;
using EmberKernel.Services.Command.Components;
using EmberKernel.Services.Command.HelpGenerator;
using EmberKernel.Services.Command.Models;
using EmberKernel.Services.Command.Sources;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Command
{
    public class CommandService : IKernelService, ICommandService
    {
        private ILogger<CommandService> Logger { get; }
        private int CommandSourceOperationTimeLimit { get; }
        private ILifetimeScope ParentScope { get; }
        private ILifetimeScope CurrentCommandScope { get; set; }
        private Dictionary<string, ILifetimeScope> CommandNamespaces { get; }
        private Dictionary<string, List<string>> CommandNamespaceAlias { get; }
        private Dictionary<string, ILifetimeScope> CommandContainerAlias { get; }
        private Dictionary<string, (string, string)> GlobalCommandAlias { get; }
        private Dictionary<ICommandContainer, ILifetimeScope> CommandScope { get; }
        private CancellationTokenSource BackgroundCancellationSource { get; set; }
        private Task CurrentRunningCommandLooping { get; set; }
        public CommandService(IConfiguration coreAppSetting, ILogger<CommandService> logger, ILifetimeScope scope)
        {
            Logger = logger;
            ParentScope = scope;
            CommandScope = new Dictionary<ICommandContainer, ILifetimeScope>();
            CommandNamespaces = new Dictionary<string, ILifetimeScope>();
            CommandNamespaceAlias = new Dictionary<string, List<string>>();
            CommandContainerAlias = new Dictionary<string, ILifetimeScope>();
            GlobalCommandAlias = new Dictionary<string, (string, string)>();


            CommandSourceOperationTimeLimit = int.Parse(coreAppSetting["CommandSourceOperationTimeLimit"] ?? "5");

            ConfigureCommandSource(builder => builder.ConfigureSource<ConsoleSource>()).AsTask().Wait();
            Keeper = new Thread(() =>
            {
                while (notDisposed) Thread.Sleep(1);
            });
            Keeper.Start();
        }

        private bool notDisposed = true;
        private readonly Thread Keeper;

        private void GlobalCommandAliasRegister(string alias, string containerName, CommandHandlerAttribute handlerAttribute)
        {
            if (GlobalCommandAlias.ContainsKey(alias))
            {
                Logger.LogWarning($"Skip conflicted global command: [{alias}] in [{containerName}]. Command alias was already been taken.");
                return;
            }
            GlobalCommandAlias.Add(alias, (containerName, handlerAttribute.Command));
        }

        private string _getCommandContainerNamespace(Type type)
        {
            var namespaceAttr = type.GetCustomAttribute<CommandContainerNamespaceAttribute>();
            if (namespaceAttr != null)
                return namespaceAttr.Namespace;
            else 
                return type.Name;
        }

        public void RegisterCommandContainer(ICommandContainer commandComponent, bool enableCommandHelp = true)
        {
            // get command component namespace
            var type = commandComponent.GetType();
            var @namespace = _getCommandContainerNamespace(type);

            // create lifecycle
            var containerScope = CurrentCommandScope.BeginLifetimeScope((builder) =>
            {
                builder.RegisterInstance(commandComponent).As<ICommandContainer>().SingleInstance();
                builder.RegisterType<CommandContainerManager>().As<ICommandContainerManager>().SingleInstance();
                if (enableCommandHelp)
                {
                    builder.RegisterType<CommandHelp>().As<ICommandHelp>().SingleInstance();
                }
            });
            CommandNamespaces.Add(@namespace, containerScope);


            // do initialize and register alias
            var manager = containerScope.Resolve<ICommandContainerManager>();
            manager.InitializeHandlers((string alias, CommandHandlerAttribute attr) => GlobalCommandAliasRegister(alias, @namespace, attr));

            var aliasAttrs = type.GetCustomAttributes<CommandContainerAliasAttribute>();
            if (aliasAttrs != null)
            {
                foreach (var aliasAttr in aliasAttrs)
                {
                    if (CommandNamespaces.ContainsKey(aliasAttr.Alias))
                    {
                        Logger.LogWarning($" Skip conflicted alias: [{aliasAttr.Alias}] in [{@namespace}]. Conflict with global namespace.");
                        continue;
                    }
                    if (CommandContainerAlias.ContainsKey(aliasAttr.Alias))
                    {
                        Logger.LogWarning($"Skip conflicted alias: [{aliasAttr.Alias}] in [{@namespace}]. Alias was already been taken.");
                        continue;
                    }
                    if (!CommandNamespaceAlias.ContainsKey(@namespace))
                    {
                        CommandNamespaceAlias.Add(@namespace, new List<string>());
                    }
                    CommandNamespaceAlias[@namespace].Add(aliasAttr.Alias);
                    CommandContainerAlias.Add(aliasAttr.Alias, containerScope);
                }
            }
        }

        public void UnregisterCommandContainer(ICommandContainer commandComponent)
        {
            var type = commandComponent.GetType();
            var @namespace = _getCommandContainerNamespace(type);
            // remove registered alias
            CommandNamespaces.Remove(@namespace, out var scope);
            if (CommandNamespaceAlias.TryGetValue(@namespace, out var aliasList))
            {
                foreach (var alias in aliasList)
                {
                    CommandContainerAlias.Remove(alias);
                }
            }

            // release scope
            if (scope != null)
            {
                scope.Dispose();
            }
        }

        public void Dispose()
        {
            DisposeAsync().AsTask().Wait();
        }
        public async ValueTask DisposeAsync()
        {
            notDisposed = false;
            Keeper.Abort();
            CommandNamespaces.Clear();
            CommandContainerAlias.Clear();
            CommandScope.Clear();
            await StopCurrentCommandLooping();
            if (CurrentCommandScope != null)
            {
                CurrentCommandScope.Dispose();
            }
        }

        public async ValueTask StopCurrentCommandLooping()
        {
            if (BackgroundCancellationSource != null && !BackgroundCancellationSource.IsCancellationRequested) BackgroundCancellationSource.Cancel();
            if (CurrentRunningCommandLooping != null) await CurrentRunningCommandLooping;
            if (CurrentCommandScope != null) using (CurrentCommandScope) { }
        }

        public async ValueTask ConfigureCommandSource(Action<ICommandSourceBuilder> builder)
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

        private async ValueTask BackgroundLooping(CancellationToken token)
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

        private ILifetimeScope _getScopeByNamespace(string @namespace)
        {
            ILifetimeScope commandScope = null;
            if (!CommandNamespaces.TryGetValue(@namespace, out commandScope))
                if (!CommandContainerAlias.TryGetValue(@namespace, out commandScope)) return null;

            return commandScope;
        }

        private void DispatchCommand(CommandArgument argument)
        {
            ILifetimeScope commandScope = _getScopeByNamespace(argument.Namespace);
            if (commandScope == null)
            {
                if (GlobalCommandAlias.TryGetValue(argument.Namespace, out var command))
                {
                    var (@namespace, cmd) = command;
                    commandScope = CommandNamespaces[@namespace];
                    if (argument.Argument == null)
                    {
                        argument.Argument = argument.Command;
                    }
                    else
                    {
                        argument.Argument = string.Join(' ', argument.Command, argument.Argument);
                    }
                    argument.Command = cmd;
                    argument.Namespace = @namespace;
                }
            }

            if (commandScope == null)
            {
                Logger.LogError($"Can't dispatch unknown command {argument.Namespace}{argument?.Command}.");
                return;
            }

            if (!commandScope.TryResolve<ICommandContainerManager>(out var manager))
            {
                Logger.LogError($"Command [{argument.Namespace}] lifescope was corrupted, can't resolve command container manager");
            }

            manager.Invoke(argument);
        }

    }
}
