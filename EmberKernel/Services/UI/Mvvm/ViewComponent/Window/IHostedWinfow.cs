using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.UI.Mvvm.ViewComponent.Window
{
    public interface IHostedWindow
    {
        Task Initialize(ILifetimeScope scope);
        Task Uninitialize(ILifetimeScope scope);
    }
}
