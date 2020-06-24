using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamplePlugin.Models.EventSubscription
{
    [EventNamespace("MemoryReader")]
    public class MultiplayerBeatmapIdInfo : Event<MultiplayerBeatmapIdInfo>
    {
        public bool HasValue { get; set; }
        public int BeatmapId { get; set; }
    }
}
