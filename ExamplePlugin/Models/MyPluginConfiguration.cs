using EmberKernel.Services.UI.Mvvm.Model;

namespace ExamplePlugin.Models
{
    public class MyPluginConfiguration
    {
        public int MyIntValue { get; set; }
        public string MyStringValue { get; set; }
        [DependencyProperty(Name = nameof(LatestBeatmapFile))]
        public string LatestBeatmapFile { get; set; }
    }
}
