using EmberKernel.Services.EventBus.Handlers;
using EmberLyricDisplayerPlugin.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmberLyricDisplayerPlugin.EventHandlers
{
    public class OsuMemoryReaderHandler : IEventHandler<BeatmapInfo>, IEventHandler<GameStatusInfo>
    {
        private readonly ILogger<OsuMemoryReaderHandler> logger;

        public OsuMemoryReaderHandler(ILogger<OsuMemoryReaderHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(BeatmapInfo @event)
        {
            throw new NotImplementedException();
        }

        public Task Handle(GameStatusInfo @event)
        {
            throw new NotImplementedException();
        }
    }
}
