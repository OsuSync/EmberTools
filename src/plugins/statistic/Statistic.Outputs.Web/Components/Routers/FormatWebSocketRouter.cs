using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic;
using SimpleHttpServer.Pipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Statistic.Outputs.Web.Components.Routers
{
    public class FormatWebSocketRouter : WebSocketMiddleware, IComponent
    {
        public ILifetimeScope Scope { get; set; }
        public IStatisticHub Hub { get; set; }
        private readonly List<Action<string, string, string>> _createdLabmda = new List<Action<string, string, string>>();
        public override async ValueTask Process(WebSocketContext wsCtx, CancellationToken cancellationToken)
        {
            void lambda(string name, string format, string value)
            {
                if (wsCtx.WebSocket.State != WebSocketState.Open)
                {
                    Hub.OnFormatUpdated -= lambda;
                    return;
                }
                wsCtx.WebSocket.SendAsync(JsonSerializer.SerializeToUtf8Bytes(new
                {
                    Name = name,
                    Value = value,
                }), WebSocketMessageType.Text, true, cancellationToken);
            }
            _createdLabmda.Add(lambda);
            Hub.OnFormatUpdated += lambda;
            await wsCtx.WebSocket.SendAsync(JsonSerializer.SerializeToUtf8Bytes((object)Hub), WebSocketMessageType.Text, true, cancellationToken);
        }

        public new void Dispose()
        {
            foreach (var lambda in _createdLabmda)
            {
                Hub.OnFormatUpdated -= lambda;
            }
            base.Dispose();
        }
    }
}
