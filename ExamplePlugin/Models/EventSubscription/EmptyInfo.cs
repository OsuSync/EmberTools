using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin.Models.EventSubscription
{
    public class EmptyInfo : Event<EmptyInfo>
    {
        public string Scarlet { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
