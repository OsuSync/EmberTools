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
            builder.ConfigureComponent<WebSockeRouter>().SingleInstance().PropertiesAutowired();
        }

        public override ValueTask Initialize(ILifetimeScope scope)
        {
            var host = scope.Resolve<WebServer>();
            var variableRouter = scope.Resolve<VariableRouter>();
            var formatRouter = scope.Resolve<FormatRouter>();
            var websocketRouter = scope.Resolve<WebSockeRouter>();
            host.AddHandlers(handle => handle
            .Use(RouterMiddleware.Route("/api/format", (route) => route.Use(variableRouter.Route)))
            .Use(RouterMiddleware.Route("/api/variable", (route) => route.Use(formatRouter.Route)))
            .Use(RouterMiddleware.Route("/ws", (route) => route.Use(websocketRouter.Route))));
            host.Run();
            return default;
        }

        public override ValueTask Uninitialize(ILifetimeScope scope)
        {
            return default;
        }
    }
}
