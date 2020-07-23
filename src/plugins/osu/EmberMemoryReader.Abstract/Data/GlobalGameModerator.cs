using EmberKernel.Services.EventBus;
using EmberMemory.Components.Collector;
using EmberMemory.Readers;
using System;

namespace EmberMemoryReader.Abstract.Data
{
    public class GlobalGameModerator : IComparableCollector<GlobalGameModeratorInfo>
    {

        private static readonly string GlobalModsPattern = "\x8B\xF1\xA1\x00\x00\x00\x00\x25\x00\x00\x40\x00\x85\xC0";
        private static readonly string GlobalModsMask = "xxx????xxxxxxx";

        public int ReadInterval { get; set; } = 500;
        public int RetryLimit { get; set; } = int.MaxValue;

        private DirectMemoryReader Reader { get; }
        public GlobalGameModerator(DirectMemoryReader reader)
        {
            Reader = reader;
        }

        private IntPtr ModAddress;
        public bool TryInitialize()
        {
            try
            {
                Reader.Reload();
                Reader.TryFindPattern(GlobalModsPattern.ToBytes(), GlobalModsMask, 3, out ModAddress);
                if (!Reader.TryReadIntPtr(ModAddress, out ModAddress)) return false;
                return true;
            }
            finally
            {
                Reader.ResetRegion();
            }
        }

        public bool TryRead(out Event result)
        {
            if (Reader.TryReadInt(ModAddress, out var listeningMods))
            {
                result = new GlobalGameModeratorInfo() { GlobalRawModerator = listeningMods };
                return true;
            }
            result = null;
            return false;
        }
    }
}
