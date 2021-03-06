﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EmberMemory.Readers
{
    public abstract class DirectMemoryReader
    {
        public Process Process { get; }
        public DirectMemoryReader(Process process)
        {
            this.Process = process;
        }
        public abstract void Reload();
        public abstract void ResetRegion();
        public abstract bool TryFindPattern(byte[] pattern, string mask, int offset, out IntPtr result);
        public abstract bool TryReadIntPtr(IntPtr address, out IntPtr value);
        public abstract bool TryReadInt(IntPtr address, out int value);
        public abstract bool TryReadShort(IntPtr address, out short value);
        public abstract bool TryReadUShort(IntPtr address, out ushort value);
        public abstract bool TryReadDouble(IntPtr address, out double value);
        public abstract bool TryReadString(IntPtr address, out string value);
        public abstract bool TryReadList<T>(IntPtr address, out List<T> value) where T : struct;
    }
}
