using Autofac;
using EmberKernel;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberMemoryReader.Abstract.Data;
using EmberMemoryReader.Abstract.Events;
using System.Threading.Tasks;

namespace PpCalculator
{
    [EmberPlugin(Author = "ZeroAsh", Name = "PpCalculator", Version = "0.1")]
    public class PpCalculatorPlugin : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<PpCalculatorService>().SingleInstance();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            scope.Subscription<GameModeInfo, PpCalculatorService>();
            scope.Subscription<BeatmapInfo, PpCalculatorService>();
            scope.Subscription<PlayingInfo, PpCalculatorService>();
            scope.Subscription<GameStatusInfo, PpCalculatorService>();
            scope.Subscription<GlobalGameModeratorInfo, PpCalculatorService>();
            scope.Subscription<OsuProcessMatchedEvent, PpCalculatorService>();

            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            scope.Unsubscription<GameModeInfo, PpCalculatorService>();
            scope.Unsubscription<BeatmapInfo, PpCalculatorService>();
            scope.Unsubscription<PlayingInfo, PpCalculatorService>();
            scope.Unsubscription<GameStatusInfo, PpCalculatorService>();
            scope.Unsubscription<GlobalGameModeratorInfo, PpCalculatorService>();
            scope.Unsubscription<OsuProcessMatchedEvent, PpCalculatorService>();

            return default;
        }
    }
}
