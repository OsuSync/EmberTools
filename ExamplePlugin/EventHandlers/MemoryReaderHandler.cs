using EmberKernel.Services.Configuration;
using EmberKernel.Services.EventBus.Handlers;
using ExamplePlugin.Models;
using ExamplePlugin.Models.EventSubscription;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePlugin.EventHandlers
{
    public class MemoryReaderHandler :
        IEventHandler<GameStatusInfo>,
        IEventHandler<BeatmapInfo>,
        IEventHandler<PlayingInfo>
    {
        private readonly ILogger<MemoryReaderHandler> _logger;
        private readonly IOptionsModerator<MyPlugin, MyPluginConfiguration> _optionsModerator;
        public MemoryReaderHandler(ILogger<MemoryReaderHandler> logger, IOptionsModerator<MyPlugin, MyPluginConfiguration> optionsModerator)
        {
            _logger = logger;
            _optionsModerator = optionsModerator;
        }

        public Task Handle(GameStatusInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current game status: {@event.StringStatus}");
            }
            else
            {
                _logger.LogInformation("Not game status found");
            }
            return Task.CompletedTask;
        }

        public async Task Handle(BeatmapInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current beatmap: SetId={@event.SetId}, BeatmapId={@event.BeatmapId}, File={@event.BeatmapFile}");
                var current = _optionsModerator.Create();
                current.LatestBeatmapFile = @event.BeatmapFile;
                await _optionsModerator.SaveAsync(current);
            }
            else
            {
                _logger.LogInformation("No beatmap found");
            }
        }

        public Task Handle(PlayingInfo @event)
        {
            if (@event.HasValue)
            {
                _logger.LogInformation($"[Event] Current playing status: time={@event.PlayingTime}, Acc={@event.GameStatistic.Accuracy}, 300={@event.GameStatistic.Combo}");
            }
            else
            {
                _logger.LogInformation("No beatmap found");
            }
            return Task.CompletedTask;
        }
    }
}
