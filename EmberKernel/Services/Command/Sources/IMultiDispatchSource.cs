namespace EmberKernel.Services.Command.Sources
{
    public interface IMultiDispatchSource : ICommandSource
    {
        void Register<T>() where T : ICommandSource;
        void Remove<T>() where T : ICommandSource;
    }
}
