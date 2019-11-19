using EmberCore.KernelServices.Command.Components;
using EmberCore.KernelServices.Command.Parsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command
{
    public interface ICommandServices
    {
        void Reigster<TCommand, TParser>(TCommand commandHandler, TParser commandParser)
            where TCommand : ICommandComponent<TParser>
            where TParser : IParser;
        void Unregister<TCommand, TParser>(TCommand commandHandler)
            where TCommand : ICommandComponent<TParser>
            where TParser : IParser;
    }
}
