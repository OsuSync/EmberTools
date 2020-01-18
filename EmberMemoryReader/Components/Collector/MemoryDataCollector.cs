using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Collector
{
    public class MemoryDataCollector : IComponent, IEventHandler<OsuProcessMatchedEvent>
    {
        Task IEventHandler<OsuProcessMatchedEvent>.Handle(OsuProcessMatchedEvent @event)
        {
            return StartCollectorAsync(@event);
        }

        public Task StartCollectorAsync(OsuProcessMatchedEvent @event)
        {
            throw new NotImplementedException();
        }
        public void Dispose() 
        {
            throw new NotImplementedException();
        }
    }
}
