using EmberKernel.Services.EventBus;
using EmberMemory.Components.Collector;
using EmberMemory.Readers;
using System;

namespace EmberMemoryReader.Abstract.Data
{
    public class GameMode : IComparableCollector<GameModeInfo>
    {
        public int ReadInterval { get; set; } = 3000;
        public int RetryLimit { get; set; } = int.MaxValue;
        
        private const string ModePattern = "\x85\xff\x74\x57\xa1\x0\x0\x0\x0\x89\x45\xe4";
        private const string ModeMask = "xxxxx????xxx";
        private IntPtr ModeAddress;
        private DirectMemoryReader Reader { get; set; }
        public GameMode(DirectMemoryReader reader)
        {
            this.Reader = reader;
        }
        public bool TryInitialize()
        {
            try
            {
                Reader.Reload();
                if (!Reader.TryFindPattern(ModePattern.ToBytes(), ModeMask, 5, out ModeAddress))
                    return false;
                if (!Reader.TryReadIntPtr(ModeAddress, out ModeAddress))
                    return false;
                if (ModeAddress == IntPtr.Zero)
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
            if (!Reader.TryReadInt(ModeAddress, out var value))
            {
                result = new GameModeInfo() { HasValue = false };
                return true;
            }
            result = new GameModeInfo() { HasValue = true, Mode = (OsuMode)value };
            return true;
        }
    }
}
