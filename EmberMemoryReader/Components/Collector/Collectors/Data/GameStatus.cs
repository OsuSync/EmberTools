using EmberKernel.Services.EventBus;
using EmberMemoryReader.Components.Collector.Readers;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    public class GameStatus : ICollector
    {
        private static readonly string GameModePattern = "\x80\xb8\x0\x0\x0\x0\x0\x75\x19\xa1\x0\x0\x0\x0\x83\xf8\x0b\x74\x0b";
        private static readonly string GameModePatternMask = "xx????xxxx????xxxxx";

        public int ReadInterval { get; set; } = 500;
        public int RetryLimit { get; set; } = int.MaxValue;
        private IntPtr GameModeAddressPtr;
        private IntPtr GameModeAddress;
        private DirectMemoryReader Reader { get; set; }
        public GameStatus(DirectMemoryReader reader)
        {
            this.Reader = reader;
        }
        public bool TryInitialize()
        {
            try
            {
                Reader.Reload();
                if (!Reader.TryFindPattern(GameModePattern.ToBytes(), GameModePatternMask, 10, out GameModeAddressPtr))
                    return false;
                if (!Reader.TryReadIntPtr(GameModeAddressPtr, out GameModeAddress))
                    return false;
                if (GameModeAddress == IntPtr.Zero)
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
            if (!Reader.TryReadInt(GameModeAddress, out var value))
            {
                result = new GameStatusInfo() { HasValue = false };
                return true;
            }
            var Status = (OsuInternalStatus)value;
            result = new GameStatusInfo() { HasValue = true, Status = Status, StringStatus = Status.ToString() };
            return true;
        }
    }
}
