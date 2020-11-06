using LyricsFinder;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberLyricDisplayerPlugin
{
    public class PluginOptions
    {
        public bool DebugMode { get; set; } = false;
        public bool EnableOutputSearchResult { get; set; } = false;
        public bool PreferTranslateLyrics { get; set; } = false;
        public string LyricsSource { get; set; } = "auto";
        public string LyricsSentenceOutputPath { get; set; } = @"..\lyric.txt";
        public uint ForceKeepTime { get; set; } = 0;
        public bool BothLyrics { get; set; } = true;
        public int GobalTimeOffset { get; set; } = 0;

        public int SearchAndDownloadTimeout { get; set; } = 2000;
        public bool StrictMatch { get; set; } = true;
    }
}
