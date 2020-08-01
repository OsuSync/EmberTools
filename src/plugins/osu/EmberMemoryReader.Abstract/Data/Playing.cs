using EmberKernel.Services.EventBus;
using EmberMemory.Components.Collector;
using EmberMemory.Readers;
using System;

namespace EmberMemoryReader.Abstract.Data
{
    public class Playing : IComparableCollector<PlayingInfo>
    {
        //0xA1,0,0,0,0,0x8D,0x56,0x08,0xE8,0,0,0,0,0x83,0x7f,0x04,0x00
        private static readonly string AccuracyPattern = "\xa1\x0\x0\x0\x0\x8d\x56\x08\xe8\x0\x0\x0\x0\x83\x7f\x04\x00";
        private static readonly string AccuracyMask = "x????xxxx????xxxx";

        //0x73,0x7a,0x8b,0x0d,0x0,0x0,0x0,0x0,0x85,0xc9,0x74,0x1f
        private static readonly string AccuracyPatternFallback = "\x73\x7a\x8b\x0d\x0\x0\x0\x0\x85\xc9\x74\x1f\x8d\x55\xf0";
        private static readonly string AccuracyMaskFallback = "xxxx????xxxxxxx";

        //0x5e,0x5f,0x5d,0xc3,0xa1,0x0,0x0,0x0,0x0,0x89,0x0,0x04
        private static readonly string TimePattern = "\x5e\x5f\x5d\xc3\xa1\x0\x0\x0\x0\x89\x0\x04";
        private static readonly string TimeMask = "xxxxx????x?x";


        public int ReadInterval { get; set; } = 120;
        public int RetryLimit { get; set; } = int.MaxValue;

        private IntPtr AccuracyAddress;
        private IntPtr TimeAddress;

        private DirectMemoryReader Reader { get; }
        public Playing(DirectMemoryReader reader)
        {
            Reader = reader;
        }

        public bool TryInitialize()
        {
            Reader.Reload();
            Reader.TryFindPattern(AccuracyPattern.ToBytes(), AccuracyMask, 1, out AccuracyAddress);
            if (!Reader.TryReadIntPtr(AccuracyAddress, out AccuracyAddress))
            {
                Reader.TryFindPattern(AccuracyPatternFallback.ToBytes(), AccuracyMaskFallback, 4, out AccuracyAddress);
                if (!Reader.TryReadIntPtr(AccuracyAddress, out AccuracyAddress)) return false;
            }

            Reader.TryFindPattern(TimePattern.ToBytes(), TimeMask, 5, out TimeAddress);
            if (!Reader.TryReadIntPtr(TimeAddress, out TimeAddress)) return false;

            Reader.ResetRegion();

            if (AccuracyAddress == IntPtr.Zero || TimeAddress == IntPtr.Zero)
            {
                return false;
            }

            return true;
        }

        private static readonly PlayingInfo EmptyInfo = new PlayingInfo() { HasValue = false };
        public bool TryRead(out Event result)
        {
            result = EmptyInfo;
            if (!Reader.TryReadIntPtr(AccuracyAddress, out var ruleSetAddress)) return false;
            if (!Reader.TryReadIntPtr(ruleSetAddress + 0x38, out var scoreAddress)) return false;
            if (!Reader.TryReadIntPtr(scoreAddress + 0x1c, out var modAddress)) return false;

            var info = new PlayingInfo() { HasValue = true };
            result = info;
            if (Reader.TryReadInt(TimeAddress, out int playingTime))
            {
                info.PlayingTime = playingTime;
            }
            if (Reader.TryReadInt(modAddress + 0x8, out int modSalt)
                && Reader.TryReadInt(modAddress + 0xc, out int mods))
            {
                info.RawModInfo = mods ^ modSalt;
            }
            if (Reader.TryReadString(scoreAddress + 0x28, out var playerName))
            {
                info.CurrentPlayerName = playerName;
            }
            //if (Reader.TryReadList<int>(scoreAddress + 0x38, out var rawUnstableList))
            //{
            //    info.RawUnstableRate = rawUnstableList;
            //}
            if (Reader.TryReadShort(scoreAddress + 0x90, out var katu))
            {
                info.Katu = katu;
            }
            if (Reader.TryReadShort(scoreAddress + 0x8e, out var geki))
            {
                info.Geki = geki;
            }
            if (Reader.TryReadShort(scoreAddress + 0x8c, out var bad))
            {
                info.Bad = bad;
            }
            if (Reader.TryReadShort(scoreAddress + 0x88, out var good))
            {
                info.Good = good;
            }
            if (Reader.TryReadShort(scoreAddress + 0x8a, out var best))
            {
                info.Best = best;
            }
            if (Reader.TryReadShort(scoreAddress + 0x92, out var missing))
            {
                info.Missing = missing;
            }
            if (Reader.TryReadShort(scoreAddress + 0x94, out var combo))
            {
                info.Combo = combo;
            }
            if (Reader.TryReadIntPtr(AccuracyAddress, out var currentScoreAddress)
                && Reader.TryReadIntPtr(currentScoreAddress + 0x44, out currentScoreAddress)
                && Reader.TryReadInt(currentScoreAddress + 0xF8, out var currentScore))
            {
                info.Score = currentScore;
            }
            if (Reader.TryReadIntPtr(ruleSetAddress + 0x40, out var hpAddress)
                && Reader.TryReadDouble(hpAddress + 0x1c, out var hp)) {
                info.HP = hp;
            }
            if (Reader.TryReadIntPtr(ruleSetAddress + 0x48, out var currentAccAddress)
                && Reader.TryReadDouble(currentAccAddress + 0x14, out double accuracy))
            {
                info.Accuracy = accuracy;
            }
            return true;

        }
    }
}
