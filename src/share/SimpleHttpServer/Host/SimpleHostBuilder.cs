using SimpleHttpServer.Pipeline;
using SimpleHttpServer.Pipeline.Middlewares;
using System;

namespace SimpleHttpServer.Host
{
    public class SimpleHostBuilder
    {
        private RequestPipeline _pipeline;
        private Action<ServerListenerBuilder> _serverBuilder = (_) => { };
        public SimpleHostBuilder Pipeline(Middleware<RequestContext> fallback, Action<IRequestPipeline<RequestContext>> builder)
        {
            _pipeline = new RequestPipeline(fallback);
            builder(_pipeline);
            return this;
        }
        public SimpleHostBuilder Pipeline(Action<IRequestPipeline<RequestContext>> builder)
        {
            return Pipeline(DefaultMiddlewares.NotFoundMiddleware, builder);
        }
        public SimpleHostBuilder ConfigureServer(Action<ServerListenerBuilder> builder)
        {
            _serverBuilder = builder;
            return this;
        }

        public SimpleHost Build()
        {
            var serverBuilder = new ServerListenerBuilder();
            if (_pipeline == null) _pipeline = new RequestPipeline(DefaultMiddlewares.NotFoundMiddleware);
            _serverBuilder(serverBuilder);
            return new SimpleHost(serverBuilder.Build(), _pipeline);
        }
    }
}
