using Autofac;
using EmberKernel.Plugins.Components;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent
{
    public interface IViewComponent : IComponent
    {
        ValueTask Initialize(ILifetimeScope scope);
        ValueTask Uninitialize(ILifetimeScope scope);
    }
}
