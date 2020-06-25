using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus.Handlers;
using MultiplayerDownloader.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerDownloader.Services
{
    public class BeatmapDownloadService : IComponent, IEventHandler<MultiplayerBeatmapIdInfo>, IEventHandler<OsuProcessMatchedEvent>
    {
        public ValueTask Handle(MultiplayerBeatmapIdInfo @event)
        {
            throw new NotImplementedException();
        }

        public ValueTask Handle(OsuProcessMatchedEvent @event)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
