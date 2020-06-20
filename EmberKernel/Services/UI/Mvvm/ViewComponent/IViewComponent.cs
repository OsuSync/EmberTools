using Autofac;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent
{
    public interface IViewComponent : IComponent
    {
        Task Initialize(ILifetimeScope scope);
        Task Uninitialize(ILifetimeScope scope);
    }
}
