﻿using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    public class BeatmapInfo : Event<BeatmapInfo>
    {
        public int BeatmapId { get; set; }
        public int SetId { get; set; }
        public string BeatmapFile { get; set; }
        public string BeatmapFolder { get; set; }
    }
}