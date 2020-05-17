using EmberKernel.Services.EventBus.Handlers;
using ExamplePlugin.Models.EventSubscription;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePlugin.EventHandlers
{
    public class MemoryReaderHandler : IEventHandler<GameStatusInfo>, IEventHandler<EmptyInfo>
    {
        private readonly ILogger<MemoryReaderHandler> _logger;
        public MemoryReaderHandler(ILogger<MemoryReaderHandler> logger)
        {
            _logger = logger;
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

        public Task Handle(EmptyInfo @event)
        {
            _logger.LogInformation("Empty event recived");
            return Task.CompletedTask;
        }
    }
}
