using Autofac;
using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemory.Components.Collector
{
    public class CollectorManagerBuilder
    {
        public ContainerBuilder Builder { get; }
        internal const string RegisteredTypesType = "RegisteredTypes";
        private readonly HashSet<Type> RegisteredCollector = new HashSet<Type>();
        public CollectorManagerBuilder(ContainerBuilder builder)
        {
            this.Builder = builder;
            Builder
                .RegisterInstance(RegisteredCollector)
                .Named<HashSet<Type>>(RegisteredTypesType);
        }

        public CollectorManagerBuilder Collect<TCollector>()
            where TCollector : ICollector
        {
            this.Builder.RegisterType<TCollector>().As<TCollector>();
            RegisteredCollector.Add(typeof(TCollector));
            return this;
        }
    }
}
