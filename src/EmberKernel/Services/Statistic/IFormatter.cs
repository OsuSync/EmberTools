using Autofac;
using EmberKernel.Services.Statistic.Format;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic
{
    public interface IFormatter : IKernelService
    {
        void Register<TContainer>(ILifetimeScope scope, string id, string format)
            where TContainer : IFormatContainer;

        void Update<TContainer>(string id, string format);

        void Unregister<TContainer>(string id)
            where TContainer : IFormatContainer;

        void UnregisterAll<TContainer>()
            where TContainer : IFormatContainer;

        bool IsRegistered<TContainer>(string id)
            where TContainer : IFormatContainer;

        IEnumerable<string> GetRegisteredFormat<TContainer>()
            where TContainer : IFormatContainer;

        string Format(string format);
    }
}
