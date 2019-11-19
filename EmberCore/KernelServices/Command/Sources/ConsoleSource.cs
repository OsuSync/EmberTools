using EmberCore.KernelServices.Command.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberCore.KernelServices.Command.Sources
{
    public sealed class ConsoleSource : ICommandSource
    {
        public void Dispose()
        {
            // nothing to dispose
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            await Task.Yield();
        }

        private readonly CommandArgument EmptyCommand = new CommandArgument() { Argument = string.Empty, Command = string.Empty };
        public async Task<CommandArgument> Read(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var result = Console.ReadLine().Split(' ', 2);
                if (result.Length == 0) return EmptyCommand;
                else
                {
                    var command = new CommandArgument() { Command = result[0] };
                    if (result.Length == 2) command.Argument = result[1];
                    return command;
                }
            }, cancellationToken);
        }

        public async Task Stop(CancellationToken cancellationToken)
        {
            await Task.Yield();
        }
    }
}
