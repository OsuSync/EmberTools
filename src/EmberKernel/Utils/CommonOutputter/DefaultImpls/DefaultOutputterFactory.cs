using EmberKernel.Utils.CommonOutputter.DefaultImpls.Outputters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Utils.CommonOutputter.DefaultImpls
{
    public class DefaultOutputterFactory : ICommonOutputterFactory
    {
        public const string MMFPREFIX = "mmf://";

        public IOutputter CreateDiskFileOutputter(string path)
        {
            return new DiskFileOutputter(path);
        }

        public IOutputter CreateMemoryMappingFileOutputter(string name)
        {
            if (name.StartsWith(MMFPREFIX))
                name = name.Substring(MMFPREFIX.Length);

            return new MemoryMapFileOutputter(name, 1024 * 1024);
        }

        public IOutputter CreateOutputterByDefinition(string name)
        {
            name = name.Trim();
            return name.StartsWith(MMFPREFIX) ? CreateMemoryMappingFileOutputter(name) : CreateDiskFileOutputter(name);
        }

        public void Dispose()
        {

        }
    }
}
