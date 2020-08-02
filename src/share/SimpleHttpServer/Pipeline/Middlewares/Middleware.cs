using System.Threading.Tasks;

namespace SimpleHttpServer.Pipeline.Middlewares
{
    public delegate ValueTask Middleware<TContext>(TContext context);
}
