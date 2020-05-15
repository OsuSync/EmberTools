using EmberKernel.Services.EventBus;
using EmberMemoryReader.Components.Collector.Readers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    public class Beatmap : ICollector
    {
        //0x83,0x3d,0x0,0x0,0x0,0x0,0x01,0x74,0x0a,0x8b,0x35,0x0,0x0,0x0,0x0,0x85,0xf6,0x75,0x04
        private static readonly string BeatmapPattern = "\x56\x8B\xF1\x8B\xFA\x8B\x0D\x00\x00\x00\x00\x85\xC9";

        private static readonly string BeatmapPatternMask = "xxxxxxx????xx";

        private static readonly int LatestBeatmapOffset = 0xc4;
        private static readonly int LatestBeatmapSetOffset= 0xc8;

        private static readonly int LatestBeatmapFolderOffset = 0x74;
        private static readonly int LatestBeatmapFilenameOffset = 0x8c;

        private int BeatmapAddressOffset { get; }
        private int BeatmapSetAddressOffset { get; }
        private int BeatmapFolderAddressOffset { get; }
        private int BeatmapFileNameAddressOffset { get; }

        private IntPtr BeatmapIdAddress;
        private IntPtr BeatmapAddress;
        private DirectMemoryReader Reader { get; set; }
        public Beatmap(DirectMemoryReader reader, OsuProcessMatchedEvent @event)
        {
            this.Reader = reader;
            BeatmapAddressOffset = LatestBeatmapOffset;
            BeatmapSetAddressOffset = LatestBeatmapSetOffset;
            BeatmapFolderAddressOffset = LatestBeatmapFolderOffset;
            BeatmapFileNameAddressOffset = LatestBeatmapFilenameOffset;

            if (@event.LatestVersion.ToComparableVersion() < "20190816".ToComparableVersion())
            {
                BeatmapAddressOffset -= 4;
                BeatmapSetAddressOffset -= 4;
                BeatmapFolderAddressOffset -= 4;
                BeatmapFileNameAddressOffset -= 4;
            }
        }

        public int ReadInterval { get; set; } = 500;

        public int RetryLimit { get; set; } = 10;

        public bool TryInitialize()
        {
            try
            {
                Reader.Reload();
                if (!Reader.TryFindPattern(BeatmapPattern.ToBytes(), BeatmapPatternMask, 7, out BeatmapIdAddress))
                    return false;
                if (!Reader.TryReadIntPtr(BeatmapIdAddress, out BeatmapAddress))
                    return false;
                if (BeatmapAddress == IntPtr.Zero)
                    return false;
                return true;
            }
            finally
            {
                Reader.ResetRegion();
            }
        }

        public bool TryRead(out Event result)
        {
            result = default;
            if (!Reader.TryReadIntPtr(BeatmapAddress, out var CurrentBeatmapAddress))
                return false;
            if (!Reader.TryReadInt(CurrentBeatmapAddress + BeatmapAddressOffset, out var beatmapId))
                return false;
            if (!Reader.TryReadInt(CurrentBeatmapAddress + BeatmapSetAddressOffset, out var setId))
                return false;
            if (!Reader.TryReadString(CurrentBeatmapAddress + BeatmapFileNameAddressOffset, out var beatmapFile))
                return false;
            if (!Reader.TryReadString(CurrentBeatmapAddress + BeatmapFolderAddressOffset, out var beatmapFolder))
                return false;
            
            result = new BeatmapInfo()
            {
                BeatmapFile = beatmapFile,
                BeatmapFolder = beatmapFolder,
                BeatmapId = beatmapId,
                SetId = setId,
            };
            return true;
        }
    }
}
