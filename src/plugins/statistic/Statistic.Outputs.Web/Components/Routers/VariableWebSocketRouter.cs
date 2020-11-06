using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic;
using EmberKernel.Services.Statistic.DataSource.Variables;
using SimpleHttpServer.Pipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Statistic.Outputs.Web.Components.Routers
{
    public class VariableWebSocketRouter : WebSocketMiddleware, IComponent
    {
        public ILifetimeScope Scope { get; set; }
        public IDataSource DataSource { get; set; }
        private readonly List<Action<IEnumerable<Variable>>> _createdLabmda = new List<Action<IEnumerable<Variable>>>();

        public override async ValueTask Process(WebSocketContext wsCtx, CancellationToken cancellationToken)
        {
            void lambda(IEnumerable<Variable> variables)
            {
                if (wsCtx.WebSocket.State != WebSocketState.Open)
                {
                    DataSource.OnMultiDataChanged -= lambda;
                    return;
                }
                wsCtx.WebSocket.SendAsync(JsonSerializer.SerializeToUtf8Bytes(variables.Select(variable => new
                {
                    variable.Id,
                    variable.Name,
                    Value = variable.Value.ToString(),
                    ValueType = variable.Value.Type,
                })), WebSocketMessageType.Text, true, cancellationToken);
            }
            _createdLabmda.Add(lambda);
            DataSource.OnMultiDataChanged += lambda;
            await wsCtx.WebSocket.SendAsync(JsonSerializer.SerializeToUtf8Bytes((object)DataSource.Select(variable => new
            {
                variable.Id,
                variable.Name,
                Value = variable.Value.ToString(),
                ValueType = variable.Value.Type,
            })), WebSocketMessageType.Text, true, cancellationToken);
        }

        public new void Dispose()
        {
            foreach (var lambda in _createdLabmda)
            {
                DataSource.OnMultiDataChanged -= lambda;
            }
            base.Dispose();
        }
    }
}
