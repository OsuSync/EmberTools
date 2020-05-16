using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.EventBus;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemoryReader.Components.Listener
{
    /// <summary>
    /// Search executable name intervally
    /// <para>Send a <see cref="ProcessSearchResult"/> message when any process match the search condition </para>
    /// </summary>
    public class ProcessListener<TPredicator, TEvent> : IComponent
        where TEvent : Event<TEvent>
        where TPredicator : IProcessPredicator<TEvent>, new()
    {
        internal int SearchDelay { get; set; }

        private ILifetimeScope CurrentScope { get; }
        private readonly IEventBus _EventBus;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private readonly CancellationToken selfToken = default;
        public ProcessListener(ILifetimeScope scope, IEventBus eventBus, IOptions<PorcessListenerConfiguration> options)
        {
            CurrentScope = scope;
            _EventBus = eventBus;
            selfToken = tokenSource.Token;
            SearchDelay = options.Value.SearchDelay;
        }

        public async Task<bool> SearchProcessAsync(CancellationToken token = default)
        {
            using var searchScope = CurrentScope.BeginLifetimeScope((builder) => builder.RegisterType<TPredicator>());
            var pred = searchScope.Resolve<TPredicator>();
            while (!token.IsCancellationRequested && !selfToken.IsCancellationRequested)
            {
                await Task.Delay(SearchDelay);
                var processes = Process.GetProcessesByName(pred.FilterProcessName);
                foreach (var process in processes)
                {
                    var result = pred.MatchProcess(process);
                    if (result != null)
                    {
                        _EventBus.Publish(result);
                        return true;
                    }
                }
            }
            return false;
        }
        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
