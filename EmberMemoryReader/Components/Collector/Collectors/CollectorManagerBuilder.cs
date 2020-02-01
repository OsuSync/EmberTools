using Autofac;
using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors
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

        public CollectorManagerBuilder Collect<TCollector, TData>()
            where TCollector : ICollector<TData>
            where TData : Event
        {
            this.Builder.RegisterType<TCollector>().As<TCollector>();
            RegisteredCollector.Add(typeof(TCollector));
            return this;
        }

        public CollectorManagerBuilder Configure<TOption>(IConfiguration config)
            where TOption : class, new()
        {
            Builder.Configure<TOption>(config);
            return this;
        }
    }
}
