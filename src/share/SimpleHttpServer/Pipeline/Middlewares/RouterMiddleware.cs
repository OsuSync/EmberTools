using System;
using System.Threading.Tasks;

namespace SimpleHttpServer.Pipeline.Middlewares
{
    public class RouterMiddleware : RequestPipeline
    {
        public RouterMiddleware(Middleware<RequestContext> fallback) : base(fallback) { }
        public RouterMiddleware() : base(DefaultMiddlewares.NotFoundMiddleware) { }

        public RouterMiddleware Post(string localPath, Middleware<RequestContext> handler)
        {
            this.Use(EndpointMiddleware.GenericHandler("POST", localPath, handler));
            return this;
        }
        public RouterMiddleware Get(string localPath, Middleware<RequestContext> handler)
        {
            this.Use(EndpointMiddleware.GenericHandler("GET", localPath, handler));
            return this;
        }
        public RouterMiddleware Put(string localPath, Middleware<RequestContext> handler)
        {
            this.Use(EndpointMiddleware.GenericHandler("PUT", localPath, handler));
            return this;
        }
        public RouterMiddleware Delete(string localPath, Middleware<RequestContext> handler)
        {
            this.Use(EndpointMiddleware.GenericHandler("DELETE", localPath, handler));
            return this;
        }

        public static Func<RequestContext, Func<ValueTask>, ValueTask> Route(string perfix, Action<RouterMiddleware> routerBuilder)
        {
            var router = new RouterMiddleware();
            routerBuilder(router);
            var routerHandler = router.Build();
            return (ctx, next) =>
            {
                if (ctx.Http.Request.Url.LocalPath.StartsWith(perfix))
                {
                    return routerHandler(ctx);
                }
                return next();
            };
        }
    }
}
