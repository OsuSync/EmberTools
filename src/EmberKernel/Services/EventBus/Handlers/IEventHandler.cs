using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Services.EventBus.Handlers
{
    public interface IEventHandler { }

    public interface IEventHandler<in TEvent> : IEventHandler
    {
        ValueTask Handle(TEvent @event);
    }
}
