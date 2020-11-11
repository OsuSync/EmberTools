using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EmberKernel.Utils.CommonOutputter.DefaultImpls.Outputters
{
    public class DiskFileOutputter : IOutputter
    {
        public string Name { get; }

        public DiskFileOutputter(string filePath) => Name = filePath;

        public async ValueTask CleanAsync() => await WriteAsync(string.Empty);

        public void Dispose()
        {

        }

        public async ValueTask WriteAsync(string content) => await File.WriteAllTextAsync(Name, content);
    }
}
