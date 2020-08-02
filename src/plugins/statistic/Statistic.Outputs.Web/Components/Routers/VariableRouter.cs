using Autofac;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic;
using SimpleHttpServer.Pipeline;
using SimpleHttpServer.Response;
using System;
using System.IO;
using System.Linq;
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
                return ctx.Http.Response.Ok(dataSource.Select(variable => new
                {
                    variable.Id,
                    variable.Name,
                    Value = variable.Value.ToString(),
                    ValueType = variable.Value.Type,
                }));
            }
            var varName = Path.GetFileName(ctx.Http.Request.Url.LocalPath);
            if (dataSource.TryGetVariable(varName, out var variable))
            {
                return ctx.Http.Response.Ok(new
                {
                    variable.Id,
                    variable.Name,
                    Value = variable.Value.ToString(),
                    ValueType = variable.Value.Type,
                });
            }
            return next();
        }
        public void Dispose() { }
    }
}
