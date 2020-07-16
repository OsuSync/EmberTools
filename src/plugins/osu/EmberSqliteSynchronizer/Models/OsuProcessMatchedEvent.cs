using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberSqliteSynchronizer.Models
{
    public class OsuProcessMatchedEvent : Event<OsuProcessMatchedEvent>
    {
        public string BeatmapDirectory { get; set; }
        public string LatestVersion { get; set; }
        public string UserName { get; set; }
        public string GameDirectory { get; set; }
        public int ProcessId { get; set; }
    }
}
