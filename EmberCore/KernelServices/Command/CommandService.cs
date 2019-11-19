using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using EmberCore.KernelServices.Command.Components;
using EmberCore.KernelServices.Command.Parsers;

namespace EmberCore.KernelServices.Command
{
    class CommandService : ICommandServices
    {
        private Dictionary<ICommandComponent, LinkedList<MethodInfo>> commandHandlers = new Dictionary<ICommandComponent, LinkedList<MethodInfo>>();

        public void Reigster(ICommandComponent commandHandler)
        {
            throw new NotImplementedException();
        }

        public void Unregister(ICommandComponent commandHandler)
        {
            throw new NotImplementedException();
        }
    }
}
