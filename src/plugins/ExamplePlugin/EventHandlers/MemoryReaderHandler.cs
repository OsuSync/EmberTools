using EmberKernel.Services.Configuration;
using EmberKernel.Services.EventBus.Handlers;
using EmberMemoryReader.Abstract.Data;
using ExamplePlugin.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExamplePlugin.EventHandlers
{
    public class MemoryReaderHandler :
        IEventHandler<GameStatusInfo>,
        IEventHandler<BeatmapInfo>,
        IEventHandler<PlayingInfo>,
        IEventHandler<MultiplayerBeatmapIdInfo>,
        IEventHandler<GameModeInfo>,
        IEventHandler<GlobalGameModeratorInfo>
    {
        private readonly ILogger<MemoryReaderHandler> _logger;
        private readonly IPluginOptions<MyPlugin, MyPluginConfiguration> _pluginOptions;
        public MemoryReaderHandler(ILogger<MemoryReaderHandler> logger, IPluginOptions<MyPlugin, MyPluginConfiguration> pluginOptions)
        {
            _logger = logger;
            _pluginOptions = pluginOptions;
        }

        public ValueTask Handle(GameStatusInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current game status: {@event.StringStatus}");
            }
            else
            {
                _logger.LogInformation("Not game status found");
            }
            return default;
        }

        public async ValueTask Handle(BeatmapInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current beatmap: SetId={@event.SetId}, BeatmapId={@event.BeatmapId}, File={@event.BeatmapFile}");
                var current = _pluginOptions.Create();
                current.LatestBeatmapFile = @event.BeatmapFile;
                await _pluginOptions.SaveAsync(current);
            }
            else
            {
                _logger.LogInformation("No beatmap found");
            }
        }

        public ValueTask Handle(PlayingInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current playing status: time={@event.PlayingTime}, Acc={@event.Accuracy}, 300={@event.Combo}");
            }
            else
            {
                _logger.LogInformation("No beatmap found");
            }
            return default;
        }

        public ValueTask Handle(MultiplayerBeatmapIdInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Multiplayer Lobby BeatmapId ={@event.BeatmapId}");
            }
            return default;
        }

        public ValueTask Handle(GameModeInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current GameMode = {@event.Mode}");
            }
            return default;
        }

        public ValueTask Handle(GlobalGameModeratorInfo @event)
        {
            _logger.LogInformation($"[Event] Current Moderator = {@event.GlobalRawModerator}");
            return default;
        }
    }
}
