using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Listener
{
    public class ProcessListenerBuilder
    {
        internal IComponentBuilder Builder { get; }
        public ProcessListenerBuilder(IComponentBuilder builder)
        {
            Builder = builder;
        }
        private Type predictor;
        private Type predictorEvent;
        public ProcessListenerBuilder UsePredictor<TPred, TEvent>()
            where TEvent : Event<TEvent>
            where TPred : IProcessPredicator<TEvent>
        {
            predictor = typeof(TPred);
            predictorEvent = typeof(TEvent);
            return this;
        }

        private Type tracker;
        private Type trackerEvent;
        public ProcessListenerBuilder UseLifetimeTracker<TTracker, TEvent>()
            where TEvent : Event<TEvent>
            where TTracker : IProcessLifetimeTracker<TEvent>
        {
            tracker = typeof(TTracker);
            trackerEvent = typeof(TEvent);
            return this;
        }

        public void Build()
        {
            if (predictor == null)
            {
                throw new MissingMethodException("Missing predictor");
            }
            if (tracker == null)
            {
                throw new MissingMethodException("Missing tracker");
            }
            var baseType = typeof(ProcessListener<,,,>);
            var concorectType = baseType.MakeGenericType(predictor, predictorEvent, tracker, trackerEvent);
            Builder.ConfigureComponent(concorectType).As<IProcessListener>();
        }
    }
}
