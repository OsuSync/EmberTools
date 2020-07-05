using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace EmberMemory.Readers.Windows
{
    public class WindowsReader : DirectMemoryReader
    {
        private SigScan Scanner { get; }
        private byte[] CreateBuffer(uint size = 8)
        {
            return new byte[size];
        }
        public WindowsReader(ILogger<SigScan> scannerLogger, Process process) : base(process)
        {
            Scanner = new SigScan(scannerLogger, process);
        }

        public override void Reload()
        {
            Scanner.Reload();
        }

        public override void ResetRegion()
        {
            Scanner.ResetRegion();
        }

        private bool TryReadByteArray(IntPtr address, uint readSize, out byte[] buffer, out int retSize)
        {
            buffer = CreateBuffer(readSize);
            if (SigScan.ReadProcessMemory(Process.Handle, address, buffer, readSize, out retSize))
            {
                return true;
            }
            return false;
        }

        private bool TryRead<T>(IntPtr address, uint size, Func<byte[], int, (T, bool)> factory, out T result)
        {
            var succeed = TryReadByteArray(address, size, out var buffer, out var retSize);
            if (succeed)
            {
                var (factoryResult, factorySucceed) = factory(buffer, retSize);
                result = factoryResult;
                return factorySucceed;
            }
            else
            {
                result = default;
            }
            return succeed;
        }

        public override bool TryReadInt(IntPtr address, out int value)
        {
            return TryRead(address, sizeof(int), (bytes, _) => (BitConverter.ToInt32(bytes, 0), true), out value);
        }

        public override bool TryReadShort(IntPtr address, out short value)
        {
            return TryRead(address, sizeof(int), (bytes, _) => (BitConverter.ToInt16(bytes, 0), true), out value);
        }

        public override bool TryReadUShort(IntPtr address, out ushort value)
        {
            return TryRead(address, sizeof(int), (bytes, _) => (BitConverter.ToUInt16(bytes, 0), true), out value);
        }

        public override bool TryFindPattern(byte[] pattern, string mask, int offset, out IntPtr result)
        {
            result = Scanner.FindPattern(pattern, mask, offset);
            if (result == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }

        public override bool TryReadDouble(IntPtr address, out double value)
        {
            return TryRead(address, sizeof(double), (bytes, _) => (BitConverter.ToDouble(bytes, 0), true), out value);
        }

        public override bool TryReadList<T>(IntPtr address, out List<T> value)
        {
            value = default;
            int typeSize = Marshal.SizeOf<T>();
            if (!TryReadIntPtr(address, out IntPtr listPtr)) return false;
            if (!TryReadInt(address + 0xc, out int length)) return false;
            if (length <= 0) return false;

            int bufferSize = typeSize * length;
            if (!TryReadIntPtr(listPtr + sizeof(int), out var arrayPtr)) return false;

            return TryRead(address, (uint)bufferSize, (bytes, retSize) =>
            {
                if (retSize != bufferSize) return (default, false);
                T[] data = new T[length];
                Buffer.BlockCopy(bytes, 0, data, 0, bufferSize);
                return (new List<T>(data), true);
            }, out value);
        }

        public override bool TryReadIntPtr(IntPtr address, out IntPtr value)
        {
            return TryRead(address, (uint)IntPtr.Size, (bytes, _) => ((IntPtr)BitConverter.ToInt32(bytes), true), out value);
        }

        public override bool TryReadString(IntPtr address, out string value)
        {
            const int MaxStringBuffer = 4096;
            value = string.Empty;
            if (!TryReadIntPtr(address, out var strBasePtr)) return false;
            if (!TryReadInt(strBasePtr + sizeof(int), out int length)) return false;

            var bufferSize = length * 2;
            if (bufferSize > MaxStringBuffer || bufferSize <= 0) return false;
            var buffer = CreateBuffer((uint)bufferSize);

            return TryRead(strBasePtr + (sizeof(int) * 2), (uint)bufferSize, (bytes, retSize) =>
            {
                if (bufferSize != retSize) return (string.Empty, false);
                return (Encoding.Unicode.GetString(bytes, 0, bufferSize), true);
            }, out value);

        }
    }
}
