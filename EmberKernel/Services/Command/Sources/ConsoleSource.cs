using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Command.Sources
{
    public sealed class ConsoleSource : ICommandSource
    {
        public void Dispose()
        {
            // nothing to dispose
        }

        public async ValueTask Initialize(CancellationToken cancellationToken)
        {
            await Task.Yield();
        }

        private readonly CommandArgument EmptyCommand = new CommandArgument() { Namespace = string.Empty, Argument = string.Empty, Command = string.Empty };
        public async ValueTask<CommandArgument> Read(CancellationToken cancellationToken)
        {
            return await Task.Run(async() =>
            {
                await Task.Yield();
                Console.Write("\n>");
                var result = Console.ReadLine().Split(' ', 3);
                if (result.Length == 0) return EmptyCommand;
                else
                {
                    var command = new CommandArgument() { Namespace = result[0] };
                    if (result.Length > 1) command.Command = result[1];
                    if (result.Length == 3) command.Argument = result[2];
                    return command;
                }
            }, cancellationToken);
        }

        public async ValueTask Stop(CancellationToken cancellationToken)
        {
            await Task.Yield();
        }
    }
}
