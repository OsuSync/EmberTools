using EmberKernel.Services.Command.Attributes;
using EmberKernel.Services.Command.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmberKernel.Services.Command
{
    public interface ICommandContainerManager : IDisposable
    {
        internal IEnumerable<(MethodInfo, CommandHandlerAttribute, Type)> ResolveHandlers();
        void InitializeHandlers(Action<string, CommandHandlerAttribute> globalCommandRegister);
        void RemoveHandlers();
        void Invoke(CommandArgument argument);
    }
}
