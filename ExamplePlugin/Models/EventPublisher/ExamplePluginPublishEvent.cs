using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin.Models.EventPublisher
{
    public class ExamplePluginPublishEvent : Event<ExamplePluginPublishEvent>
    {
        public int InputNumber { get; set; }
    }
}
