﻿using EmberKernel.Services.EventBus.Handlers;
using ExamplePlugin.Models.EventSubscription;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExamplePlugin.EventHandlers
{
    public class ExamplePluginPublishEventHandler : IEventHandler<ExamplePluginPublishEvent>
    {
        private readonly ILogger<ExamplePluginPublishEventHandler> _logger;
        public ExamplePluginPublishEventHandler(ILogger<ExamplePluginPublishEventHandler> logger)
        {
            _logger = logger;
        }

        public ValueTask Handle(ExamplePluginPublishEvent @event)
        {
            _logger.LogInformation($" !!! Event handled !! -> value = {@event.InputNumber}");
            return default;
        }
    }
}
