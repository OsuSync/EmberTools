namespace EmberKernel.Services.Statistic.Notifier
{
    public interface INotifierManager
    {
        void RegisterNotifier<TNotifier>() where TNotifier : INotifier;
        void RegisterNotifier<TNotifier>(TNotifier nofifier) where TNotifier : INotifier;
        void UnregisterNotifier<TNotifier>() where TNotifier : INotifier;
    }
}
