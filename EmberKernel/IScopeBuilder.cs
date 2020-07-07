using Autofac;
using System.Threading.Tasks;

namespace EmberKernel
{
    public interface IScopeBuilder
    {
        void BuildScope(ContainerBuilder builder);
        ValueTask Run(ILifetimeScope scope);
    }
}
