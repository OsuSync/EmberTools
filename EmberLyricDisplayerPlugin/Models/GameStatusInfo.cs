using EmberKernel.Services.EventBus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EmberLyricDisplayerPlugin.Models
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

    public class GameStatusInfo : Event<GameStatusInfo>, IComparable<GameStatusInfo>, IEquatable<GameStatusInfo>
    {
        public bool HasValue { get; set; }
        public OsuInternalStatus Status { get; set; }
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
