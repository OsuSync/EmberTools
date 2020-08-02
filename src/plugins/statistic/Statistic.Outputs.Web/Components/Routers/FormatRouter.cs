using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic;
using SimpleHttpServer.Pipeline;
using SimpleHttpServer.Response;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Statistic.Outputs.Web.Components.Routers
{
    public class FormatRouter : IComponent
    {
        public ILifetimeScope Scope { get; set; }
        public ValueTask Route(RequestContext ctx, Func<ValueTask> next)
        {
            var hub = Scope.Resolve<IStatisticHub>();
            if (ctx.Http.Request.Url.LocalPath == "/api/format")
            {
                return ctx.Http.Response.Ok(hub);
            }
            var formatName = Path.GetFileName(ctx.Http.Request.Url.LocalPath);
            if (hub.IsRegistered(formatName))
            {
                return ctx.Http.Response.Ok(hub.GetValue(formatName));
            }
            return next();
        }
        public void Dispose() { }
    }
}
