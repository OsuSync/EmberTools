using EmberKernel.Plugins.Components;
using EmberKernel.Services.Command;
using EmberKernel.Services.EventBus.Handlers;
using EmberLyricDisplayerPlugin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OsuSqliteDatabase.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ATL;
using LyricsFinder;
using EmberKernel.Services.Configuration;

namespace EmberLyricDisplayerPlugin.Kernel
{
    public class LyricsController : IComponent, IEventHandler<BeatmapInfo>, IEventHandler<GameStatusInfo>, IEventHandler<PlayingInfo>
    {
        private readonly LyricsFinder lyricsFinder;
        private readonly ILogger<LyricsController> logger;
        private readonly OsuDatabaseContext osuDb;
        private readonly LyricsOutputter outputter;
        private readonly PluginOptions option;
        private OsuInternalStatus prevStatus = OsuInternalStatus.Unknown;
        private int? currentBeatmapId = null;

        private Lyrics currentLyrics;
        private int prevSentenceIndex = -1;
        private int prevTime = -1;

        public LyricsController(
            LyricsFinder lyricsFinder,
            ILogger<LyricsController> logger,
            OsuDatabaseContext osuDb,
            LyricsOutputter outputter,
            IReadOnlyPluginOptions<PluginOptions> optionFactory)
        {
            this.lyricsFinder = lyricsFinder;
            this.logger = logger;
            this.osuDb = osuDb;
            this.outputter = outputter;
            option = optionFactory.Create();
        }

        public ValueTask Handle(BeatmapInfo evt)
        {
            currentBeatmapId = evt?.BeatmapId;
            return default;
        }

        public async ValueTask Handle(GameStatusInfo evt)
        {
            if (prevStatus == evt.Status)
                return;

            switch (evt.Status)
            {
                case OsuInternalStatus.Play:
                    await SetupLyricsDisplay();
                    break;
                default:

                    break;
            }

            prevStatus = evt.Status;
        }

        private async ValueTask SetupLyricsDisplay()
        {
            if ((!(currentBeatmapId is int bid)) || lyricsFinder is null)
                return;

            //get actual beatmap info from osu db.
            var beatmapInfo = await osuDb.OsuDatabaseBeatmap.FirstOrDefaultAsync(x => x.BeatmapId == bid);
            if (beatmapInfo is null)
            {
                logger.LogDebug("beatmapInfo 为空，无法从数据库获取详细谱面信息");
                return;
            }

            var osuFilePath = beatmapInfo.FileName;
            logger.LogDebug($"osuFilePath : {osuFilePath}");

            var duration = await GetDuration(osuFilePath);

            if (duration <= 0)
                return;

            var lyrics = await lyricsFinder.GetLyrics(beatmapInfo.TitleUnicode, beatmapInfo.ArtistUnicode, duration);

            if (lyrics is null)
            {
                logger.LogInformation($"谱面 ({bid}) {beatmapInfo.ArtistUnicode} - {beatmapInfo.TitleUnicode} 没有歌词");
                return;
            }

            currentLyrics = lyrics;
        }

        private async ValueTask<int> GetDuration(string osuFile)
        {
            if (!File.Exists(osuFile))
                return -1;

            var parent = Directory.GetParent(osuFile).FullName;

            var audio_path = Path.Combine(parent, await GetAudioFilePath(osuFile));

            if (audio_path == null)
                return -1;

            var track = new Track(audio_path);

            logger.LogDebug("duration:" + track.Duration);

            return track.Duration * 1000;//convert to ms
        }

        private async Task<string> GetAudioFilePath(string osuFilePath)
        {
            var lines = await File.ReadAllLinesAsync(osuFilePath);

            foreach (var line in lines)
            {
                if (line.StartsWith("AudioFilename:"))
                    return line.Replace("AudioFilename:", string.Empty).Trim();

                if (line.Contains("[Editor]"))
                    return string.Empty;
            }

            return string.Empty;
        }

        public void Dispose()
        {

        }

        public ValueTask Handle(PlayingInfo evt)
        {
            var time = evt.PlayingTime;
            if (currentLyrics is null || time == prevTime)
                return default;

            time += option.GobalTimeOffset;

            var (sentence,sentenceIndex) = currentLyrics.GetCurrentSentence(time);

            if (prevSentenceIndex != sentenceIndex)
            {
                logger.LogDebug($"[cur : {time} , idx : {prevSentenceIndex} -> {sentenceIndex} , sentence : {sentence.StartTime}]{sentence.Content}");
                prevSentenceIndex = sentenceIndex;
            }
            else
            {
                if (option.ForceKeepTime != 0 && time - sentence.StartTime > option.ForceKeepTime)
                    sentence = Sentence.Empty;
            }

            OutputLyricSentence(sentence);

            prevTime = time;
            return default;
        }

        private void OutputLyricSentence(Sentence sentence)
        {
            throw new NotImplementedException();
        }
    }
}
