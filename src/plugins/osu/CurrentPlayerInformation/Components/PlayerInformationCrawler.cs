using CurrentPlayerInformation.Models;
using CurrentPlayerInformation.Util;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Abstract.Data;
using EmberMemoryReader.Abstract.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CurrentPlayerInformation.Components
{
    public class PlayerInformationCrawler : IComponent,
        IEventHandler<OsuProcessMatchedEvent>,
        IEventHandler<PlayingInfo>
    {
        public ILogger<PlayerInformationCrawler> Logger { get; set; }
        public IEventBus EventBus { get; set; }
        private readonly HttpClient _requestor = new HttpClient();
        private readonly Dictionary<string, (PlayerInformation, DateTime)> _informationCache = new Dictionary<string, (PlayerInformation, DateTime)>();
        private string lastPlayerName = null;

        private readonly static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = SnakeCase.Policy,
        };

        private class SnakeCase : JsonNamingPolicy
        {
            public readonly static SnakeCase Policy = new SnakeCase();
            public override string ConvertName(string name)
            {
                return name.ToSnakeCase();
            }
        }

        private async ValueTask<(bool, PlayerInformation)> TryCrawlerPlayerInformation(string playerName)
        {
            Logger.LogInformation($"Start crawl the information of player {playerName}");
            if (_informationCache.ContainsKey(playerName))
            {
                var (cachedInfo, crawledAt) = _informationCache[playerName];
                if ((DateTime.Now - crawledAt).TotalMinutes < 30)
                    return (true, cachedInfo);
            }
            using var res = await _requestor.GetAsync($"https://osu.ppy.sh/users/{playerName}");
            if (!res.IsSuccessStatusCode) { return (false, null); }
            var rawHtml = await res.Content.ReadAsStringAsync();
            var userJsonStartPos = rawHtml.IndexOf(">", rawHtml.LastIndexOf("json-user")) + 1;
            var userJsonEndPos = rawHtml.IndexOf("</script>", userJsonStartPos);
            var userJson = rawHtml[userJsonStartPos..userJsonEndPos];
            
            var info = JsonSerializer.Deserialize<PlayerInformation>(userJson, SerializerOptions);
            _informationCache[playerName] = (info, DateTime.Now);
            return (true, info);
        }

        public async ValueTask ProcessCurrentPlayerName(string playerName)
        {
            if (playerName == null) return;
            if (lastPlayerName != playerName)
            {
                lastPlayerName = playerName;
                var (succeed, info) = await TryCrawlerPlayerInformation(playerName);
                if (succeed)
                {
                    EventBus.Publish(PlayerInformationEvent.FromPlayerInformation(info));
                    return;
                }
                else
                {
                    Logger.LogInformation($"Crawl failed! May player {playerName} not exist.");
                }
            }
        }

        public ValueTask Handle(OsuProcessMatchedEvent @event)
        {
            _ = ProcessCurrentPlayerName(@event.UserName);
            return default;
        }

        public void Dispose()
        {
            _requestor.Dispose();
        }

        public ValueTask Handle(PlayingInfo @event)
        {
            return ProcessCurrentPlayerName(@event.CurrentPlayerName);
        }
    }
}
