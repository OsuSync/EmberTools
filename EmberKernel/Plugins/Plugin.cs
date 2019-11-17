using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using EmberKernel.Plugins.Components;

namespace EmberKernel.Plugins
{
    public abstract class Plugin : IPlugin
    {
        public abstract void BuildComponents(IComponentBuilder builder);
        public abstract void Initialize(ILifetimeScope scope);
    }
}
