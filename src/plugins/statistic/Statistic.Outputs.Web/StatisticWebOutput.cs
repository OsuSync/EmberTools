using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using SimpleHttpServer.Pipeline;
using SimpleHttpServer.Pipeline.Middlewares;
using Statistic.Outputs.Web.Components;
using Statistic.Outputs.Web.Components.Routers;
using System.Threading.Tasks;

namespace Statistic.Outputs.Web
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Statistic Exporter - Web", Version = "1.0")]
    public class StatisticWebOutput : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<WebServer>().SingleInstance();
            builder.ConfigureComponent<VariableRouter>().SingleInstance().PropertiesAutowired();
            builder.ConfigureComponent<FormatRouter>().SingleInstance().PropertiesAutowired();
            builder.ConfigureComponent<FormatWebSocketRouter>().SingleInstance().PropertiesAutowired();
            builder.ConfigureComponent<VariableWebSocketRouter>().SingleInstance().PropertiesAutowired();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            var host = scope.Resolve<WebServer>();
            var variableRouter = scope.Resolve<VariableRouter>();
            var formatRouter = scope.Resolve<FormatRouter>();
            var formatWebsocketRouter = scope.Resolve<FormatWebSocketRouter>();
            var variableWebsocketRouter = scope.Resolve<VariableWebSocketRouter>();
            host.AddHandlers(handle => handle
                .Use(RouterMiddleware.Route("/api/format", (route) => route.Use(formatRouter.Route)))
                .Use(RouterMiddleware.Route("/api/variable", (route) => route.Use(variableRouter.Route)))
                .Use(RouterMiddleware.Route("/ws/format", (route) => route.Use(formatWebsocketRouter.Route)))
                .Use(RouterMiddleware.Route("/ws/variable", (route) => route.Use(variableWebsocketRouter.Route)))
            );
            host.Run();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            var host = scope.Resolve<WebServer>();
            host.Stop();
            return default;
        }
    }
}
