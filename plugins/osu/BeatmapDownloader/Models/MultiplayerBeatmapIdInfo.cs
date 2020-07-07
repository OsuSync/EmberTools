using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeatmapDownloader.Models
{
    [EventNamespace("MemoryReader")]
    public class MultiplayerBeatmapIdInfo : Event<MultiplayerBeatmapIdInfo>
    {
        public bool HasValue { get; set; }
        public int BeatmapId { get; set; }
    }
}
