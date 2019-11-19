using System;
using System.Collections.Generic;
using System.Text;

namespace EmberCore.KernelServices.Command.Builder
{
    public interface ICommandSourceBuilder
    {
        ICommandSourceBuilder ConfigureSource<T>();
        ICommandSourceBuilder ConfigureSource(Type type);
        ICommandSourceBuilder ConfigureMultiSource();
    }
}
