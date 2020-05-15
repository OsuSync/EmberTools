using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using ExamplePlugin.Models.EventSubscription;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamplePlugin.EventHandlers
{
    public class MemoryReaderEmptyEventHandler : IEventHandler<EmptyInfo>
    {
        private static DateTimeOffset StartTime;
        private static int Counter = 0;
        static MemoryReaderEmptyEventHandler()
        {
            StartTime = DateTimeOffset.Now;
        }
        private readonly ILogger<MemoryReaderEmptyEventHandler> _logger;
        public MemoryReaderEmptyEventHandler(ILogger<MemoryReaderEmptyEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(EmptyInfo @event)
        {
            var allTime = (DateTimeOffset.Now - StartTime).TotalMilliseconds;
            var time = allTime / (++Counter);
            Console.WriteLine($"{allTime}ms, Speed = {1000 / (time)}msg/s, {time}ms/message");
            return Task.CompletedTask;
        }
    }
}
