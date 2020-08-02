using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHttpServer.Pipeline.Middlewares
{
    public abstract class WebSocketMiddleware : IDisposable
    {
        private readonly List<HttpListenerWebSocketContext> _createdWs = new List<HttpListenerWebSocketContext>();
        private readonly CancellationTokenSource privateTokenSource = new CancellationTokenSource();

        public void Dispose()
        {
            privateTokenSource.Cancel();
            foreach (var ws in _createdWs)
            {
                ws.WebSocket.Abort();
            }
        }

        public async ValueTask Route(RequestContext ctx, Func<ValueTask> next)
        {
            if (!ctx.Http.Request.IsWebSocketRequest)
            {
                await next();
                return;
            }
            var wsContext = await ctx.Http.AcceptWebSocketAsync(null);
            _createdWs.Add(wsContext);
            await Process(wsContext, privateTokenSource.Token);
        }

        public abstract ValueTask Process(WebSocketContext wsContext, CancellationToken token);

    }
}
