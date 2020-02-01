using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmberMemoryReader.Components.Collector.Collectors.Data
{
    public enum OsuInternalStatus
    {
        Menu,
        Edit,
        Play,
        Exit,
        SelectEdit,
        SelectPlay,
        SelectDrawings,
        Rank,
        Update,
        Busy,
        Unknown,
        Lobby,
        MatchSetup,
        SelectMulti,
        RankingVs,
        OnlineSelection,
        OptionsOffsetWizard,
        RankingTagCoop,
        RankingTeam,
        BeatmapImport,
        PackageUpdater,
        Benchmark,
        Tourney,
        Charts
    };
    public class GameStatusInfo : Event<GameStatusInfo>
    {
        public OsuInternalStatus Status { get; set; }
    }
}
