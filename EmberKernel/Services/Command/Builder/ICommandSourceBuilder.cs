using System;

namespace EmberKernel.Services.Command.Builder
{
    public interface ICommandSourceBuilder
    {
        ICommandSourceBuilder ConfigureSource<T>();
        ICommandSourceBuilder ConfigureSource(Type type);
        ICommandSourceBuilder ConfigureMultiSource();
    }
}
