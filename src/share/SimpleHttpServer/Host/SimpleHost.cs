using SimpleHttpServer.Pipeline;
using SimpleHttpServer.Pipeline.Middlewares;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHttpServer.Host
{
    public class SimpleHost
    {
        private RequestPipeline Pipeline { get; }
        private Middleware<RequestContext> Handler { get; set; }
        public ServerListener Server { get; }
        public SimpleHost(ServerListener server, RequestPipeline pipeline)
        {
            Server = server;
            Pipeline = pipeline;
            Handler = Pipeline.Build();
            Server.ProcessRequest += Server_ProcessRequest;
        }

        public void AddHandlers(Action<IRequestPipeline<RequestContext>> builder)
        {
            builder(Pipeline);
            Handler = Pipeline.Build();
        }

        private ValueTask Server_ProcessRequest(HttpListenerContext context, CancellationToken cancellationToken = default)
        {
            return Handler(new RequestContext() { Http = context, CancelToken = cancellationToken });
        }

        public async ValueTask Run(CancellationToken cancellationToken = default)
        {
            await Server.HandleRequest(cancellationToken);
        }
    }
}
