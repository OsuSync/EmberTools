using EmberKernel.Plugins.Components;
using Microsoft.Extensions.Logging;
using SimpleHttpServer.Host;
using SimpleHttpServer.Pipeline;
using System;

namespace Statistic.Outputs.Web.Components
{
    public class WebServer : IComponent
    {
        private ILogger<StatisticWebOutput> Logger { get; }
        private SimpleHost Host { get; }
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
            _ = Host.Run();
            Logger.LogInformation($"HTTP server listen on {string.Join(",", Host.Server.Listener.Prefixes)}");
        }
        public void Dispose()
        {
            Host.Dispose();
        }
    }
}
