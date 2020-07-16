using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Services.Configuration
{
    public interface IReadOnlyPluginOptions<TOptions>
        where TOptions : class, new()
    {
        public TOptions Create();
    }
}
