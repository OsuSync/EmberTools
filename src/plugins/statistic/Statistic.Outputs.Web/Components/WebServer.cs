using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Logging;
using SimpleHttpServer.Host;
using SimpleHttpServer.Pipeline;
using System;
using System.Threading;

namespace Statistic.Outputs.Web.Components
{
    public class WebServer : IComponent
    {
        private ILogger<StatisticWebOutput> Logger { get; }
        private SimpleHost Host { get; }
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public WebServer(ILogger<StatisticWebOutput> logger)
        {
            Logger = logger;
            Host = new SimpleHostBuilder()
                .ConfigureServer((server) => server.ListenLocalPort(11111))
                .Build();
        }
        public void AddHandlers(Action<IRequestPipeline<RequestContext>> builder)
        {
            Host.AddHandlers(builder);
        }
        public void Run()
        {
            _ = Host.Run(_cancellationTokenSource.Token);
            Logger.LogInformation($"HTTP server listen on {string.Join(",", Host.Server.Listener.Prefixes)}");
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            Host.Stop();
        }
        public void Dispose()
        {
            Host.Dispose();
        }
    }
}
