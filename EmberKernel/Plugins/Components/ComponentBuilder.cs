using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins.Components
{
    public class ComponentBuilder : IComponentBuilder
    {
        internal ContainerBuilder Container { get; }
        public ComponentBuilder(ContainerBuilder container)
        {
            Container = container;
        }

        public void ConfigureComponent<TComponent>() where TComponent : IComponent
        {
            Container.RegisterType<TComponent>();
        }

        public void ConfigureComponent<TComponent>(TComponent instance) where TComponent : class, IComponent
        {
            Container.RegisterInstance(instance);
        }

        public void ConfigureComponent<TComponent>(Func<TComponent> factory) where TComponent : IComponent
        {
            Container.Register((_) => factory());
        }

        internal void ConfigureComponent<TComponent>(Func<IComponentContext, TComponent> factory) where TComponent : IComponent
        {
            Container.Register(factory);
        }
    }
}
