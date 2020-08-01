using Autofac;
using System;
using System.Threading.Tasks;

namespace EmberKernel
{
    public interface IScopeBuilder : IDisposable, IAsyncDisposable
    {
        void BuildScope(ContainerBuilder builder);
        ValueTask Run(ILifetimeScope scope);
    }
}
