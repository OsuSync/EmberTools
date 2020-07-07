using Autofac;
using Autofac.Builder;
using System;

namespace EmberKernel.Plugins.Components
{
    public class ComponentBuilder : IComponentBuilder
    {
        internal ContainerBuilder Container { get; }
        internal ILifetimeScope ParentScope { get; }
        ContainerBuilder IComponentBuilder.Container => Container;
        ILifetimeScope IComponentBuilder.ParentScope => ParentScope;

        public ComponentBuilder(ContainerBuilder container, ILifetimeScope parentScope)
        {
            Container = container;
            ParentScope = parentScope;
        }

        public IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> ConfigureComponent(Type type)
        {
            if (!type.IsAssignableTo<IComponent>())
            {
                throw new NotImplementedException($"Type {nameof(type)} not implemented type {nameof(IComponent)}");
            }
            return Container.RegisterType(type);
        }

        public IRegistrationBuilder<TComponent, ConcreteReflectionActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>() where TComponent : IComponent
        {
            return Container.RegisterType<TComponent>();
        }

        public IRegistrationBuilder<TComponent, SimpleActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>(TComponent instance) where TComponent : class, IComponent
        {
            return Container.RegisterInstance(instance);
        }

        public IRegistrationBuilder<TComponent, SimpleActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>(Func<TComponent> factory) where TComponent : IComponent
        {
            return Container.Register((_) => factory());
        }

        internal IRegistrationBuilder<TComponent, SimpleActivatorData, SingleRegistrationStyle> ConfigureComponent<TComponent>(Func<IComponentContext, TComponent> factory) where TComponent : IComponent
        {
            return Container.Register(factory);
        }
    }
}
