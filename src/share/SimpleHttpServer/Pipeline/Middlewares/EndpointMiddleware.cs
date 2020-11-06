using System;
using System.Threading.Tasks;

namespace SimpleHttpServer.Pipeline.Middlewares
{
    public class EndpointMiddleware
    {
        public static Func<RequestContext, Func<ValueTask>, ValueTask> GenericHandler(string httpMethod, string localPath, Middleware<RequestContext> handler)
        {
            return (ctx, next) =>
            {
                if (ctx.Http.Request.HttpMethod.ToLower() == httpMethod.ToLower())
                {
                    if (ctx.Http.Request.Url.LocalPath.EndsWith(localPath))
                    {
                        return handler(ctx);
                    }
                }
                return next();
            };
        }
    }
}
