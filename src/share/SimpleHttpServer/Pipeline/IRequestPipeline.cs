using SimpleHttpServer.Pipeline.Middlewares;
using System;

namespace SimpleHttpServer.Pipeline
{
    public interface IRequestPipeline<TContext>
    {
        IRequestPipeline<TContext> Use(Func<Middleware<TContext>, Middleware<TContext>> middleware);
        Middleware<TContext> Build();
    }
    public interface IRequestPipeline : IRequestPipeline<RequestContext>
    {
    }
}
