using EmberCore.KernelServices.PluginResolver.Loader;
using EmberKernel;
using EmberKernel.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EmberCore.KernelServices.PluginResolver
{
    public class CorePluginResolver : KernelService
    {
        private IOptionsSnapshot<CoreAppSetting> CoreAppSetting { get; }
        private ILogger<CorePluginResolver> Logger { get; }
        private IContentRoot ContentRoot { get; }
        private readonly Dictionary<string, LoaderContext> LoadedContexts = new Dictionary<string, LoaderContext>();
        public CorePluginResolver(IOptionsSnapshot<CoreAppSetting> coreAppSetting, ILogger<CorePluginResolver> logger, IContentRoot contentRoot)
        {
            CoreAppSetting = coreAppSetting;
            Logger = logger;
            ContentRoot = contentRoot;
        }

        private void CreateCache(string srcPath, string targetPath)
        {
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            var dirsToProcess = new Queue<(string sourcePath, string destinationPath)>();
            dirsToProcess.Enqueue((sourcePath: srcPath, destinationPath: targetPath));
            while (dirsToProcess.Any())
            {
                (string sourcePath, string destinationPath) = dirsToProcess.Dequeue();

                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                var sourceDirectoryInfo = new DirectoryInfo(sourcePath);
                foreach (FileInfo sourceFileInfo in sourceDirectoryInfo.EnumerateFiles())
                    sourceFileInfo.CopyTo(Path.Combine(destinationPath, sourceFileInfo.Name), true);

                foreach (DirectoryInfo sourceSubDirectoryInfo in sourceDirectoryInfo.EnumerateDirectories())
                    dirsToProcess.Enqueue((
                        sourcePath: sourceSubDirectoryInfo.FullName,
                        destinationPath: Path.Combine(destinationPath, sourceSubDirectoryInfo.Name)));
            }
        }

        public IEnumerable<ILoaderContext> EnumerateLoaders()
        {
            var pluginBaseFolder = Path.Combine(ContentRoot.ContentDirectory, CoreAppSetting.Value.PluginsFolder);
            var cacheFolder = Path.Combine(ContentRoot.ContentDirectory, CoreAppSetting.Value.PluginsCacheFolder);
            var sourcePluginFolders = Directory.EnumerateDirectories(pluginBaseFolder);
            try
            {
                Directory.Delete(cacheFolder, true);
            }
            catch { }
            foreach (var folder in sourcePluginFolders)
            {
                string cache = Path.Combine(cacheFolder, $"{Path.GetFileName(folder)}_{Path.GetRandomFileName()}");
                CreateCache(folder, cache);
                LoadedContexts.Add(folder, new LoaderContext(cache, Logger));
                yield return LoadedContexts[folder];
            }
        }
    }
}
