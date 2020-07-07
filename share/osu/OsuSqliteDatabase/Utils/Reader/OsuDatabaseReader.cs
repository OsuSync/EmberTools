using OsuSqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OsuSqliteDatabase.Utils.Reader
{
    public class OsuDatabaseReader : IDisposable
    {
        private BinaryReader Reader { get; }
        public OsuDatabaseReader(string path)
        {
            Reader = new BinaryReader(File.OpenRead(path), Encoding.UTF8);
        }

        public OsuDatabase ReadOsuDatabaseHead()
        {
            return new OsuDatabase
            {
                Version = Reader.ReadInt32(),
                FolderCount = Reader.ReadInt32(),
                AccountLocked = Reader.ReadBoolean(),
                UnlockedAt = Reader.ReadDateTime(),
                PlayerName = Reader.ReadOsuString(),
                BeatmapCount = Reader.ReadInt32()
            };
        }

        public OsuDatabase ReadOsuDatabase(ref OsuDatabase db)
        {
            db.Beatmaps = ReadBeatmap(db).ToList();
            db.Permission = (OsuGameBeatmapPermission)Reader.ReadInt32();
            return db;
        }

        private IEnumerable<OsuDatabaseBeatmap> ReadBeatmap(OsuDatabase db)
        {
            for (int i = 0; i < db.BeatmapCount; i++)
            {
                var beatmap = new OsuDatabaseBeatmap();
                if (db.Version < 20191106) beatmap.BytesOfBeatmapEntry = Reader.ReadInt32();
                beatmap.Artist = Reader.ReadOsuString();
                beatmap.ArtistUnicode = Reader.ReadOsuString();
                beatmap.Title = Reader.ReadOsuString();
                beatmap.TitleUnicode = Reader.ReadOsuString();
                beatmap.Creator = Reader.ReadOsuString();
                beatmap.Difficult = Reader.ReadOsuString();
                beatmap.AudioFileName = Reader.ReadOsuString();
                beatmap.MD5Hash = Reader.ReadOsuString();
                beatmap.FileName = Reader.ReadOsuString();
                beatmap.RankStatus = (OsuGameBeatmapRankStatus)Reader.ReadByte();
                beatmap.CircleCount = Reader.ReadInt16();
                beatmap.SliderCount = Reader.ReadInt16();
                beatmap.SpinnerCount = Reader.ReadInt16();
                beatmap.LatestModifiedAt = Reader.ReadDateTime();
                beatmap.ApproachRate = db.Version >= 20140609 ? Reader.ReadSingle() : Reader.ReadByte();
                beatmap.CircleSize = db.Version >= 20140609 ? Reader.ReadSingle() : Reader.ReadByte();
                beatmap.HPDrain = db.Version >= 20140609 ? Reader.ReadSingle() : Reader.ReadByte();
                beatmap.OverallDifficulty = db.Version >= 20140609 ? Reader.ReadSingle() : Reader.ReadByte();
                beatmap.SliderVelocity = Reader.ReadDouble();
                if (db.Version >= 20140609)
                {
                    beatmap.StarRatings = ReadStarRatings(beatmap).ToList();
                }
                beatmap.DrainTime = Reader.ReadInt32();
                beatmap.TotalTime = Reader.ReadInt32();
                beatmap.AudioPreviewTime = Reader.ReadInt32();
                beatmap.TimingPointCount = Reader.ReadInt32();
                beatmap.OsuDatabaseTimings = ReadBeatmapTimings(beatmap).ToList();
                beatmap.BeatmapId = Reader.ReadInt32();
                beatmap.BeatmapSetId = Reader.ReadInt32();
                beatmap.ThreadId = Reader.ReadInt32();
                beatmap.StandardRankRating = (OsuGameRankRating)Reader.ReadByte();
                beatmap.TaikoRankRating = (OsuGameRankRating)Reader.ReadByte();
                beatmap.CatchTheBeatRankRating = (OsuGameRankRating)Reader.ReadByte();
                beatmap.ManiaRankRating = (OsuGameRankRating)Reader.ReadByte();
                beatmap.LocalOffset = Reader.ReadInt16();
                beatmap.StackLeniency = Reader.ReadSingle();
                beatmap.RuleSet = (OsuGameRuleSet)Reader.ReadByte();
                beatmap.Source = Reader.ReadOsuString();
                beatmap.Tags = Reader.ReadOsuString();
                beatmap.OnlineOffset = Reader.ReadInt16();
                beatmap.TitleFont = Reader.ReadOsuString();
                beatmap.NotPlayed = Reader.ReadBoolean();
                beatmap.LatestPlayedAt = Reader.ReadDateTime();
                beatmap.IsOsz2 = Reader.ReadBoolean();
                beatmap.FolderName = Reader.ReadOsuString();
                beatmap.LatestPlayedAt = Reader.ReadDateTime();
                beatmap.BeatmapSoundIgnored = Reader.ReadBoolean();
                beatmap.BeatmapSkinIgnored = Reader.ReadBoolean();
                beatmap.StoryboardDisabled = Reader.ReadBoolean();
                beatmap.VideoDisabled = Reader.ReadBoolean();
                beatmap.VisualOverrided = Reader.ReadBoolean();
                if (db.Version < 20140609)
                    Reader.BaseStream.Seek(sizeof(short), SeekOrigin.Current);
                Reader.BaseStream.Seek(sizeof(int), SeekOrigin.Current);
                beatmap.ManiaScrollSpeed = Reader.ReadByte();

                yield return beatmap;
            }
        }

        private IEnumerable<OsuDatabaseBeatmapStarRating> ReadSingleModeRatings(OsuDatabaseBeatmap beatmap, OsuGameRuleSet ruleSet)
        {
            foreach (var (moderator, rate) in Reader.ReadDictionary<OsuGameModerator, double>())
            {
                yield return new OsuDatabaseBeatmapStarRating()
                {
                    Moderators = moderator,
                    OsuDatabaseBeatmap = beatmap,
                    RuleSet = ruleSet,
                    StarRating = rate,
                };
            }
        }

        private IEnumerable<OsuDatabaseBeatmapStarRating> ReadStarRatings(OsuDatabaseBeatmap beatmap)
        {
            return ReadSingleModeRatings(beatmap, OsuGameRuleSet.Standard)
                .Concat(ReadSingleModeRatings(beatmap, OsuGameRuleSet.Taiko))
                .Concat(ReadSingleModeRatings(beatmap, OsuGameRuleSet.Catch))
                .Concat(ReadSingleModeRatings(beatmap, OsuGameRuleSet.Mania));
        }

        private IEnumerable<OsuDatabaseTimings> ReadBeatmapTimings(OsuDatabaseBeatmap beatmap)
        {
            for (int i = 0; i < beatmap.TimingPointCount; i++)
            {
                yield return new OsuDatabaseTimings()
                {
                    BeatPreMinute = Reader.ReadDouble(),
                    Offset = Reader.ReadDouble(),
                    Inherited = Reader.ReadBoolean(),
                    OsuDatabaseBeatmap = beatmap,
                };
            }
        }

        public void Dispose()
        {
            Reader.Dispose();
        }
    }
}
