using Autofac;
using System;

namespace EmberKernel.Services.EventBus.SubscriptionManager
{
    public class SubscriptionInfo
    {
        public bool IsDynamic { get; }
        public Type HandlerType { get; }
        internal ILifetimeScope Scope { get; }

        private SubscriptionInfo(bool isDynamic, Type handlerType, ILifetimeScope scope)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
            Scope = scope;
        }

        public static SubscriptionInfo Dynamic(Type handlerType, ILifetimeScope scope)
        {
            return new SubscriptionInfo(true, handlerType, scope);
        }
        public static SubscriptionInfo Typed(Type handlerType, ILifetimeScope scope)
        {
            return new SubscriptionInfo(false, handlerType, scope);
        }
    }
}
