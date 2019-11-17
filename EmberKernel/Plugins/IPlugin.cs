using Autofac;
using EmberKernel.Plugins.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins
{
    public interface IPlugin
    {
        void BuildComponents(IComponentBuilder builder);
        void Initialize(ILifetimeScope scope);
    }
}
