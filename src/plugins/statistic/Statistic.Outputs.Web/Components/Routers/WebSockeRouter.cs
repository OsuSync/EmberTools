using Autofac;
using EmberKernel.Plugins.Components;
using SimpleHttpServer.Pipeline;
using System;
using System.Threading.Tasks;

namespace Statistic.Outputs.Web.Components.Routers
{
    public class WebSockeRouter : IComponent
    {
        public ILifetimeScope Scope { get; set; }
        public ValueTask Route(RequestContext ctx, Func<ValueTask> next)
        {
            if (!ctx.Http.Request.IsWebSocketRequest)
            {
                return next();
            }

        }
        public void Dispose() { }
    }
}
