using EmberKernel.Services.Statistic.Format;

namespace EmberKernel.Services.Statistic
{
    public interface IFormatter
    {
        void Register<TContainer>(string format)
            where TContainer : IFormatContainer;

        void Unregister<TContainer>(string format)
            where TContainer : IFormatContainer;

        void IsRegistered<TContainer>(string format)
            where TContainer : IFormatContainer;

        string Format(string format);
    }
}
