using EmberKernel.Plugins.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberKernel.Plugins.Models
{
    public class PluginDescriptor
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public static PluginDescriptor FromAttribute(EmberPluginAttribute attr)
        {
            return new PluginDescriptor()
            {
                Author = attr.Author,
                Name = attr.Name,
                Version = attr.Version,
            };
        }

        public override string ToString()
        {
            return $"{Name}({Author}) - {Version}";
        }
    }
}
