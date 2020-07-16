using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin.Models.EventSubscription
{
    [EventNamespace("MemoryReader")]
    public class GameStatusInfo : Event<GameStatusInfo>
    {
        public bool HasValue { get; set; }
        public int Status { get; set; }
        public string StringStatus { get; set; }
    }
}
