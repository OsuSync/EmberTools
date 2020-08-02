using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.Statistic;
using SimpleHttpServer.Host;
using SimpleHttpServer.Pipeline;
using SimpleHttpServer.Pipeline.Middlewares;
using SimpleHttpServer.Response;
using System.IO;
using System.Threading.Tasks;

namespace Statistic.Outputs.Web
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Statistic Exporter - Web", Version = "1.0")]
    public class StatisticWebOutput : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.Container.RegisterInstance(
                new SimpleHostBuilder()
                .ConfigureServer((server) => server.ListenLocalPort(11111))
                .Build())
            .AsSelf()
            .SingleInstance();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            var host = scope.Resolve<SimpleHost>();
            host.AddHandlers(handle => handle
            .Use(RouterMiddleware.Route("/api/format", formatRoute => formatRoute
                .Use((ctx, next) =>
                {
                    var hub = scope.Resolve<IStatisticHub>();
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
                })
            ))
            .Use(RouterMiddleware.Route("/api/variable", varRoute => varRoute
                .Use((ctx, next) =>
                {
                    var dataSource = scope.Resolve<IDataSource>();
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
                }))
            ));
            _ = host.Run();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            return default;
        }
    }
}
