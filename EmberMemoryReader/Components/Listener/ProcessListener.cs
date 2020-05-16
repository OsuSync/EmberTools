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
        internal TPredicator Pred { get; set; }

        private readonly IEventBus _EventBus;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();
        private readonly CancellationToken selfToken = default;
        public ProcessListener(IEventBus eventBus, IOptions<PorcessListenerConfiguration> options)
        {
            _EventBus = eventBus;
            Pred = new TPredicator();
            selfToken = tokenSource.Token;
            SearchDelay = options.Value.SearchDelay;
        }

        public async Task<bool> SearchProcessAsync(CancellationToken token = default)
        {
            try
            {
                while (!token.IsCancellationRequested && !selfToken.IsCancellationRequested)
                {
                    await Task.Delay(SearchDelay);
                    var processes = Process.GetProcessesByName(Pred.FilterProcessName);
                    foreach (var process in processes)
                    {
                        var result = Pred.MatchProcess(process);
                        if (result != null)
                        {
                            _EventBus.Publish(result);
                            return true;
                        }
                    }
                }
                return false;
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            } 
        }
        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
