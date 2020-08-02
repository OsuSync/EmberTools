using SimpleHttpServer.Response;
using System.Threading.Tasks;

namespace SimpleHttpServer.Pipeline.Middlewares
{
    public static class DefaultMiddlewares
    {
        public static ValueTask NotFoundMiddleware(RequestContext context)
        {
            return context.Http.Response.NotFound();
        }
    }
}
