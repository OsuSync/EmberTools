using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins.Components
{
    public interface IComponentBuilder
    {
        void ConfigureComponent<TComponent>() where TComponent : IComponent;
        void ConfigureComponent<TComponent>(TComponent instance) where TComponent : class, IComponent;
        void ConfigureComponent<TComponent>(Func<TComponent> factory) where TComponent : IComponent;
    }
}
