using EmberKernel.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberKernel.Services.Configuration
{
    public interface IOptionsModerator<TPlugin, TOptions>
        where TPlugin : Plugin
        where TOptions : class, new()
    {
        public TOptions Create();
        public Task SaveAsync(TOptions options, CancellationToken cancellationToken = default);
    }
}
