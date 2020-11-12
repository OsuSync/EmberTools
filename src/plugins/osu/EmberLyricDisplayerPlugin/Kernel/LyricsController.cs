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
using EmberKernel.Utils.CommonOutputter;

namespace EmberLyricDisplayerPlugin.Kernel
{
    public class LyricsController : IComponent, IEventHandler<BeatmapInfo>, IEventHandler<GameStatusInfo>, IEventHandler<PlayingInfo>
    {
        private readonly LyricsFinder lyricsFinder;
        private readonly ILogger<LyricsController> logger;
        private readonly OsuDatabaseContext osuDb;
        private readonly PluginOptions option;
        private OsuInternalStatus prevStatus = OsuInternalStatus.Unknown;
        private int? currentBeatmapId = null;

        private Lyrics currentLyrics = default;
        private int prevSentenceIndex = -1;
        private int prevTime = -1;
        private IOutputter outputter;

        public LyricsController(
            LyricsFinder lyricsFinder,
            ILogger<LyricsController> logger,
            OsuDatabaseContext osuDb,
            ICommonOutputterFactory outputterFactory,
            IReadOnlyPluginOptions<PluginOptions> optionFactory)
        {
            this.lyricsFinder = lyricsFinder;
            this.logger = logger;
            this.osuDb = osuDb;
            option = optionFactory.Create();

            var outputPath = option.LyricsSentenceOutputPath;

            try
            {
                //init outputer
                outputter = outputterFactory.CreateOutputterByDefinition(outputPath);
                outputter.CleanAsync();
                logger.LogInformation($"create {outputter.GetType().Name} to output target : {outputter.Name}");
            }
            catch (Exception e)
            {
                logger.LogError($"Can't create any outputter for output target : {outputPath} , err msg : {e.Message}");
            }
        }

        public ValueTask Handle(BeatmapInfo evt)
        {
            currentBeatmapId = evt?.BeatmapId;
            logger.LogDebug($"currentBeatmapId : {currentBeatmapId?.ToString()}");
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
                    CleanLyricsDisplay();
                    break;
            }

            logger.LogDebug($"status {prevStatus} -> {evt.Status}");
            prevStatus = evt.Status;
        }

        private void CleanLyricsDisplay()
        {
            currentBeatmapId = null;
            currentLyrics = null;
            prevSentenceIndex = -1;
            prevTime = -1;
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
            logger.LogInformation($"osuFilePath : {duration}");

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

        private async ValueTask<string> GetAudioFilePath(string osuFilePath)
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
            outputter?.Dispose();
        }

        public async ValueTask Handle(PlayingInfo evt)
        {
            var time = evt.PlayingTime;
            if (currentLyrics is null || time == prevTime)
                return;

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

            await OutputLyricSentence(sentence);

            prevTime = time;
        }

        private ValueTask OutputLyricSentence(Sentence sentence) => outputter?.WriteAsync(sentence.Content) ?? default;
    }
}
