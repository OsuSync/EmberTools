using EmberKernel.Services.EventBus;
using EmberMemoryReader.Components.Collector.Readers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    class MultiplayerBeatmapId : IComparableCollector<MultiplayerBeatmapIdInfo>
    {
        public int ReadInterval { get; set; } = 1500;
        public int RetryLimit { get; set; } = int.MaxValue;

        private DirectMemoryReader Reader { get; set; }
        public MultiplayerBeatmapId(DirectMemoryReader reader)
        {
            this.Reader = reader;
        }

        private IntPtr MultiplayerBaseAddress;
        private const string MultiplayerPattern = "\x8b\x4c\x98\x08\xa1\x0\x0\x0\x0\x8b\x50\x0c\x3b\x5a\x04";
        private const string MultiplayerBeatmapPatternMask = "xxxxx????xxxxxx";
        private const int BeatmapIdOffset = 0x2c;


        public bool TryInitialize()
        {
            try
            {
                Reader.Reload();
                if (!Reader.TryFindPattern(MultiplayerPattern.ToBytes(), MultiplayerBeatmapPatternMask, 5, out MultiplayerBaseAddress))
                    return false;
                if (!Reader.TryReadIntPtr(MultiplayerBaseAddress, out MultiplayerBaseAddress))
                    return false;
                return true;
            }
            finally
            {
                Reader.ResetRegion();
            }
        }

        private static readonly MultiplayerBeatmapIdInfo EmptyResult = new MultiplayerBeatmapIdInfo() { HasValue = false };
        public bool TryRead(out Event result)
        {
            result = EmptyResult;
            if (!Reader.TryReadIntPtr(MultiplayerBaseAddress, out var MultiplayerAddress))
                return false;
            if (!Reader.TryReadInt(MultiplayerAddress + BeatmapIdOffset, out int value))
                return false;
            result = new MultiplayerBeatmapIdInfo() { BeatmapId = value, HasValue = value > 0 };
            return true;
        }
    }
}
