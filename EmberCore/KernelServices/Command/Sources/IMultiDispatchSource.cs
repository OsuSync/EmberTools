using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Sources
{
    public interface IMultiDispatchSource : ICommandSource
    {
        void Register<T>() where T : ICommandSource;
        void Remove<T>() where T : ICommandSource;
    }
}
