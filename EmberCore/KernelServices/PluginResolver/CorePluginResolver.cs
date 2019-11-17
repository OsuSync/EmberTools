using EmberCore.KernelServices.PluginResolver.Loader;
using EmberKernel;
using EmberKernel.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace EmberCore.KernelServices.PluginResolver
{
    public class CorePluginResolver : KernelService
    {
        private IOptionsSnapshot<CoreAppSetting> CoreAppSetting { get; }
        private ILogger Logger { get; }
        private IContentRoot ContentRoot { get; }
        public CorePluginResolver(IOptionsSnapshot<CoreAppSetting> coreAppSetting, ILogger logger, IContentRoot contentRoot)
        {
            CoreAppSetting = coreAppSetting;
            Logger = logger;
            ContentRoot = contentRoot;
        }

        public IEnumerable<ILoaderContext> EnumerableLoaders()
        {
            var pluginBaseFolder = CoreAppSetting.Value.PluginsFolder;
            var pluginFolders = Directory.EnumerateDirectories(Path.Combine(ContentRoot.ContentDirectory, pluginBaseFolder));
            foreach (var folder in pluginFolders)
            {
                yield return new LoaderContext(folder, Logger);
            }
        }
    }
}
