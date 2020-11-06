using EmberKernel.Services.EventBus;
using EmberKernel.Services.Statistic.DataSource.Variables;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmberMemoryReader.Abstract.Data
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
    }

    [EventNamespace("MemoryReader")]
    public class GameStatusInfo : Event<GameStatusInfo>, IComparable<GameStatusInfo>, IEquatable<GameStatusInfo>
    {
        public bool HasValue { get; set; }
        [DataSourceVariable]
        public OsuInternalStatus Status { get; set; }
        [DataSourceVariable(Name = "GameStatus")]
        public string StringStatus { get; set; }

        public int CompareTo([AllowNull] GameStatusInfo other)
        {
            return (int)this.Status - (int)other.Status;
        }

        public bool Equals([AllowNull] GameStatusInfo other)
        {
            return this.Status == other.Status;
        }
    }
}
