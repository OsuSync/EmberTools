using System;
using System.Threading.Tasks;

namespace SimpleHttpServer.Pipeline
{
    public static class RequestPipelineExtension
    {
        public static IRequestPipeline<TContext> Use<TContext>(this IRequestPipeline<TContext> builder, Func<TContext, Func<ValueTask>, ValueTask> func)
        {
            return builder.Use(next => context => func(context, () => next(context)));
        }
    }
}
