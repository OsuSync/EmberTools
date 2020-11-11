using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Utils.CommonOutputter.DefaultImpls.Outputters
{
    public class MemoryMapFileOutputter : IOutputter
    {
        private static readonly byte[] CleanBuffer = new byte[]{ 0 };

        public string Name { get; }

        private MemoryMappedFile mmf;

        public MemoryMapFileOutputter(string name,int capacity)
        {
            Name = name;
            mmf = MemoryMappedFile.CreateOrOpen(name, capacity);
        }

        public async ValueTask CleanAsync() => await WriteAsync(string.Empty);

        public async ValueTask WriteAsync(string content)
        {
            using var stream = mmf.CreateViewStream();
            var valueBytes = Encoding.UTF8.GetBytes(content);
            await stream.WriteAsync(valueBytes);
            await stream.WriteAsync(CleanBuffer);
            await stream.FlushAsync();
        }

        public void Dispose() => mmf.Dispose();
    }
}
