using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberLyricDisplayerPlugin.Lyrics
{
    public class LyricsFinder : IComponent
    {
        public LyricsFinder(IOptionsSnapshot<PluginOptions> option)
        {
            this.option = option;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
