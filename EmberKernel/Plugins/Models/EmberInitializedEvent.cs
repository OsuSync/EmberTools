using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins.Models
{
    public class EmberInitializedEvent : Event<EmberInitializedEvent>
    {
        public static EmberInitializedEvent Empty = new EmberInitializedEvent();
    }
}
