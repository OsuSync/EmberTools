using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Abstract.Data;
using EmberMemoryReader.Abstract.Events;
using Microsoft.Extensions.Logging;
using SimpleOsuPerformanceCalculator.Calculator;
using System.IO;
using System.Threading.Tasks;

namespace PpCalculator
{
    public class PpCalculatorService : IComponent,
        IEventHandler<GameModeInfo>,
        IEventHandler<BeatmapInfo>,
        IEventHandler<PlayingInfo>,
        IEventHandler<GameStatusInfo>,
        IEventHandler<GlobalGameModeratorInfo>,
        IEventHandler<OsuProcessMatchedEvent>
    {
        private SimplePerformanceCalculator Calculator { get; set; }
        private OsuMode LatestGameMode { get; set; }
        private string CurrentBeatmapPath { get; set; }
        private string OsuBeatmapPath { get; set; } = null;
        private PlayingInfo CurrentPlayingInfo { get; set; }
        private int LatestGameModerator { get; set; }
        private int PlayingModerator { get; set; }
        private int CurrentMaxCombo { get; set; }

        private IEventBus EventBus { get; }
        private ILogger<PpCalculatorService> Logger { get; }
        public PpCalculatorService(IEventBus eventBus, ILogger<PpCalculatorService> logger)
        {
            EventBus = eventBus;
            Logger = logger;
        }
        public void Dispose()
        {
        }

        private void RegenerateCalculator()
        {
            if (Calculator != null)
                Calculator.PerformanceChanged -= Calculator_PerformanceChanged;
            Calculator = new SimplePerformanceCalculator((SupportModes)LatestGameMode, CurrentBeatmapPath);
            CurrentMaxCombo = 0;
            Calculator.UpdateModerator(LatestGameModerator);
            Calculator.PerformanceChanged += Calculator_PerformanceChanged;
            DoScoreUpdate(ignoreCurrentStatistic: true);
        }

        private void Calculator_PerformanceChanged(double pp)
        {
            Logger.LogInformation($"Current PP {pp}");
        }

        private void DoScoreUpdate(bool ignoreCurrentStatistic = false)
        {
            if (OsuBeatmapPath == null || Calculator == null) return;
            if (ignoreCurrentStatistic || !CurrentPlayingInfo.HasValue)
            {
                Calculator.SetCurrentOffset(int.MaxValue);
                switch (LatestGameMode)
                {
                    case OsuMode.Osu:
                        Calculator.UpdateOsuScore(Calculator.Max300, 0, 0, 0, 1, Calculator.MaxCombo);
                        break;
                    case OsuMode.Taiko:
                        Calculator.UpdateTaikoScore(Calculator.Max300, 0, 0, 0);
                        break;
                    // unsupported now
                    //case OsuMode.CatchTheBeat:
                    //    Calculator.UpdateCatchScore(Calculator.Max300, 0, 0, 0, 1, Calculator.MaxCombo);
                    //    break;
                    case OsuMode.Mania:
                        Calculator.UpdateManiaScore(1000000, Calculator.Max300, 0, 0, 0, 0, 0, 1, Calculator.MaxCombo);
                        break;
                    default:
                        break;
                }
                return;
            }
            else
            {
                Calculator.SetCurrentOffset(CurrentPlayingInfo.PlayingTime);
                if (PlayingModerator != CurrentPlayingInfo.RawModInfo)
                {
                    PlayingModerator = CurrentPlayingInfo.RawModInfo;
                    Calculator.UpdateModerator(PlayingModerator);
                }
                var statistic = CurrentPlayingInfo.GameStatistic;
                if (statistic.Combo > CurrentMaxCombo)
                {
                    CurrentMaxCombo = statistic.Combo;
                }
                switch (LatestGameMode)
                {
                    case OsuMode.Osu:
                        Calculator.UpdateOsuScore(
                            statistic.Best,
                            statistic.Good,
                            statistic.Bad,
                            statistic.Missing,
                            statistic.Accuracy / 100,
                            CurrentMaxCombo);
                        break;
                    case OsuMode.Taiko:
                        Calculator.UpdateTaikoScore(
                            statistic.Best,
                            statistic.Good,
                            statistic.Bad,
                            statistic.Missing);
                        break;
                    // unsupported now
                    //case OsuMode.CatchTheBeat:
                    //    Calculator.UpdateCatchScore(Calculator.Max300, 0, 0, 0, 1, Calculator.MaxCombo);
                    //    break;
                    case OsuMode.Mania:
                        Calculator.UpdateManiaScore(
                            statistic.Score,
                            statistic.Best,
                            statistic.Geki,
                            statistic.Good,
                            statistic.Katu,
                            statistic.Bad,
                            statistic.Missing,
                            statistic.Accuracy,
                            statistic.Combo);
                        break;
                    default:
                        break;
                }
                return;
            }
        }

        public ValueTask Handle(GameStatusInfo @event)
        {
            if (@event.HasValue)
            {
                if (@event.Status != OsuInternalStatus.Play || @event.Status != OsuInternalStatus.Rank)
                {
                    DoScoreUpdate(ignoreCurrentStatistic: true);
                }
            }
            return default;
        }

        public ValueTask Handle(PlayingInfo @event)
        {
            CurrentPlayingInfo = @event;
            DoScoreUpdate();
            return default;
        }

        public ValueTask Handle(GameModeInfo @event)
        {
            if (@event.HasValue)
            {
                LatestGameMode = @event.Mode;
            }
            return default;
        }

        public ValueTask Handle(BeatmapInfo @event)
        {
            if (@event.HasValue)
            {
                CurrentBeatmapPath = Path.Combine(OsuBeatmapPath, @event.BeatmapFolder, @event.BeatmapFile);
                RegenerateCalculator();
            }
            return default;
        }

        public ValueTask Handle(OsuProcessMatchedEvent @event)
        {
            this.OsuBeatmapPath = Path.Combine(@event.GameDirectory, @event.BeatmapDirectory);
            return default;
        }

        public ValueTask Handle(GlobalGameModeratorInfo @event)
        {
            LatestGameModerator = @event.GlobalRawModerator;
            if (OsuBeatmapPath == null || Calculator == null) return default;
            Calculator.UpdateModerator(LatestGameModerator);
            return default;
        }
    }
}
