using Autofac;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Plugins
{
    public interface IPlugin
    {
        void BuildComponents(IComponentBuilder builder);
        Task Initialize(ILifetimeScope scope);
        Task Uninitialize(ILifetimeScope scope);
    }
}
