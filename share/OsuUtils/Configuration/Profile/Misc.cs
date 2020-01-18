using System;
using System.Collections.Generic;
using System.Text;

namespace OsuUtils.Configuration.Profile
{
    public interface IMisc
    {
        public string BeatmapDirectory { get; }
        public string LastVersion { get; }
        public string ChatChannels { get; }
    }
}
