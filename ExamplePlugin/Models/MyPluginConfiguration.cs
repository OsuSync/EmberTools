using EmberKernel.Services.UI.Mvvm.Dependency;
using EmberKernel.Services.UI.Mvvm.ViewModel;

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
