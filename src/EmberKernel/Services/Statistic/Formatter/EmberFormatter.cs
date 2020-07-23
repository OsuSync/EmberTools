using EmberKernel.Services.Statistic.Format;
using System;

namespace EmberKernel.Services.Statistic.Formatter
{
    public class EmberFormatter : IFormatter
    {
        public string Format(string format)
        {
            throw new NotImplementedException();
        }

        public void IsRegistered<TContainer>(string format) where TContainer : IFormatContainer
        {
            throw new NotImplementedException();
        }

        public void Register<TContainer>(string format) where TContainer : IFormatContainer
        {
            throw new NotImplementedException();
        }

        public void Unregister<TContainer>(string format) where TContainer : IFormatContainer
        {
            throw new NotImplementedException();
        }
    }
}
