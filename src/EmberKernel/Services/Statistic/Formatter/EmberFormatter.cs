using Autofac;
using EmberKernel.Services.Statistic.Format;
using System.Collections.Generic;

namespace EmberKernel.Services.Statistic.Formatter
{
    public class EmberFormatter : IFormatter
    {
        public string Format(string format)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> GetRegisteredFormat<TContainer>() where TContainer : IFormatContainer
        {
            throw new System.NotImplementedException();
        }

        public void IsRegistered<TContainer>(string format) where TContainer : IFormatContainer
        {
            throw new System.NotImplementedException();
        }

        public void Register<TContainer>(ILifetimeScope scope, string format) where TContainer : IFormatContainer
        {
            throw new System.NotImplementedException();
        }

        public void Unregister<TContainer>(string format) where TContainer : IFormatContainer
        {
            throw new System.NotImplementedException();
        }

        public void UnregisterAll<TContainer>() where TContainer : IFormatContainer
        {
            throw new System.NotImplementedException();
        }
    }
}
