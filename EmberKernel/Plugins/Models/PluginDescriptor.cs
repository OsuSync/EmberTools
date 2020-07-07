using EmberKernel.Plugins.Attributes;

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

        public override bool Equals(object obj)
        {
            return this.GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}({Author}) - {Version}";
        }
    }
}
