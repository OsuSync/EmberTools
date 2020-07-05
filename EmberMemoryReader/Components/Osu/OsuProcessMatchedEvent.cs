﻿using EmberKernel.Services.EventBus;
using EmberMemory.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmberMemoryReader.Components.Osu
{
    public class OsuProcessMatchedEvent : ProcessMatchedEvent<OsuProcessMatchedEvent>
    {
        public string BeatmapDirectory { get; set; }
        public string LatestVersion { get; set; }
        public string UserName { get; set; }
        public string GameDirectory { get; set; }
    }
}
