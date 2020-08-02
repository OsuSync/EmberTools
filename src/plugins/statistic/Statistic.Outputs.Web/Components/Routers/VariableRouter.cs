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
    public class VariableRouter : IComponent
    {
        public ILifetimeScope Scope { get; set; }
        public ValueTask Route(RequestContext ctx, Func<ValueTask> next)
        {
            var dataSource = Scope.Resolve<IDataSource>();
            if (ctx.Http.Request.Url.LocalPath == "/api/variable")
            {
                return ctx.Http.Response.Ok(dataSource);
            }
            var varName = Path.GetFileName(ctx.Http.Request.Url.LocalPath);
            if (dataSource.TryGetVariable(varName, out var variable))
            {
                return ctx.Http.Response.Ok(variable);
            }
            return next();
        }
        public void Dispose() { }
    }
}
