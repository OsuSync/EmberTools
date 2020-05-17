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
        private static readonly string BeatmapPattern = "\xDB\x5D\xE8\x8B\x45\xE8\xA3\x0\x0\x0\x0\x8B\x35\x0\x0\x0\x0\x85\xF6";

        private static readonly string BeatmapPatternMask = "xxxxxxx????xx????xx";

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

        public int RetryLimit { get; set; } = int.MaxValue;

        public bool TryInitialize()
        {
            try
            {
                Reader.Reload();
                if (!Reader.TryFindPattern(BeatmapPattern.ToBytes(), BeatmapPatternMask, 13, out BeatmapIdAddress))
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

        private static readonly BeatmapInfo EmptyResult = new BeatmapInfo() { HasValue = false };
        public bool TryRead(out Event result)
        {
            result = EmptyResult;
            if (!Reader.TryReadIntPtr(BeatmapAddress, out var CurrentBeatmapAddress))
                return true;
            if (!Reader.TryReadInt(CurrentBeatmapAddress + BeatmapAddressOffset, out var beatmapId))
                return true;
            if (!Reader.TryReadInt(CurrentBeatmapAddress + BeatmapSetAddressOffset, out var setId))
                return true;
            Reader.TryReadString(CurrentBeatmapAddress + BeatmapFileNameAddressOffset, out var beatmapFile);
            //return true;
            Reader.TryReadString(CurrentBeatmapAddress + BeatmapFolderAddressOffset, out var beatmapFolder);
                // return true;
            
            result = new BeatmapInfo()
            {
                BeatmapFile = beatmapFile,
                BeatmapFolder = beatmapFolder,
                BeatmapId = beatmapId,
                SetId = setId,
                HasValue = true,
            };
            return true;
        }
    }
}
