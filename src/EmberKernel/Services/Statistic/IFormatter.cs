using Autofac;
using EmberKernel.Services.Statistic.Format;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic
{
    public interface IFormatter
    {
        void Register<TContainer>(ILifetimeScope scope, string format)
            where TContainer : IFormatContainer;

        void Unregister<TContainer>(string format)
            where TContainer : IFormatContainer;

        void UnregisterAll<TContainer>()
            where TContainer : IFormatContainer;

        bool IsRegistered<TContainer>(string format)
            where TContainer : IFormatContainer;

        IEnumerable<string> GetRegisteredFormat<TContainer>()
            where TContainer : IFormatContainer;

        string Format(string format);
    }
}
