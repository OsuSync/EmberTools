using Autofac;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins.Components
{
    public interface IComponentBuilder
    {
        IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> ConfigureComponent(Type type);
        IRegistrationBuilder<TComponent, ConcreteReflectionActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>() where TComponent : IComponent;
        IRegistrationBuilder<TComponent, SimpleActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>(TComponent instance) where TComponent : class, IComponent;
        IRegistrationBuilder<TComponent, SimpleActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>(Func<TComponent> factory) where TComponent : IComponent;
    }
}
