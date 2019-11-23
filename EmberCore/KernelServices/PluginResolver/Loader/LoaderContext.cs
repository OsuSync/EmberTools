using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace EmberCore.KernelServices.PluginResolver.Loader
{
    internal class LoaderContext : AssemblyLoadContext, ILoaderContext
    {
        private string FolderPath { get; }
        private ILogger Logger { get; }

        public string CurrentPath => FolderPath;

        public LoaderContext(string folder, ILogger logger)
        {
            FolderPath = folder;
            Logger = logger;
            if (!Directory.Exists(FolderPath))
            {
                throw new FileNotFoundException(FolderPath);
            }
        }

        public IEnumerable<Assembly> LoadAssemblies()
        {
            var dllFiles = Directory.EnumerateFiles(FolderPath, "*.plugin");
            foreach (var dllPath in dllFiles)
            {
                Assembly asm = default;
                try
                {
                    asm = LoadFromAssemblyPath(Path.Combine(FolderPath, dllPath));
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, $"Unknown plugin: {dllFiles}");
                }
                if (asm != null) yield return asm;
            }
            
        }
    }
}
