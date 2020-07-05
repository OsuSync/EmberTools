using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Plugins.Models;
using EmberKernel.Services.EventBus;
using EmberKernel.Services.EventBus.Handlers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmberMemory.Listener
{
    /// <summary>
    /// Search executable name intervally
    /// <para>Send a <see cref="ProcessSearchResult"/> message when any process match the search condition </para>
    /// </summary>
    public class ProcessListener<TPredicator, TPredEvent, TLifeTrakcer, TLifeEvent>
        : IProcessListener
        where TPredEvent : Event<TPredEvent>
        where TPredicator : IProcessPredicator<TPredEvent>
        where TLifeEvent : Event<TLifeEvent>
        where TLifeTrakcer : IProcessLifetimeTracker<TLifeEvent>
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

        public async ValueTask Handle(EmberInitializedEvent _)
        {
            // search osu! process
            var listener = CurrentScope.Resolve<IProcessListener>();
            await listener.SearchProcessAsync(tokenSource.Token);
        }

        public async ValueTask SearchProcessAsync(CancellationToken token = default)
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
                        await EnsureProcessLifetime(process, token);
                    }
                }
            }
        }

        public async ValueTask EnsureProcessLifetime(Process process, CancellationToken token = default)
        {
            using var lifetimeScope = CurrentScope.BeginLifetimeScope((builder) => builder.RegisterType<TLifeTrakcer>());
            var tracker = lifetimeScope.Resolve<TLifeTrakcer>();
            while (!token.IsCancellationRequested && !selfToken.IsCancellationRequested)
            {
                await Task.Delay(SearchDelay);
                var result = tracker.Report(process);
                if (result.Terminated)
                {
                    _EventBus.Publish(result.Report);
                    return;
                }
            }
        }

        public void Dispose()
        {
            tokenSource.Cancel();
        }
    }
}
