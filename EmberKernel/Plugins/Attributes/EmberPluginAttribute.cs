using System;

namespace EmberKernel.Plugins.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EmberPluginAttribute : Attribute
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }

        public override string ToString()
        {
            return $"{Name}({Author}) - {Version}";
        }
    }
}
