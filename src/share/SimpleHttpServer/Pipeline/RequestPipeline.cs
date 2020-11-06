using SimpleHttpServer.Pipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleHttpServer.Pipeline
{
    public class RequestPipeline : IRequestPipeline<RequestContext>
    {
        private readonly Middleware<RequestContext> _fallback;
        private readonly IList<Func<Middleware<RequestContext>, Middleware<RequestContext>>> _pipelines =
            new List<Func<Middleware<RequestContext>, Middleware<RequestContext>>>();

        public RequestPipeline(Middleware<RequestContext> fallback)
        {
            _fallback = fallback;
        }

        public IRequestPipeline<RequestContext> Use(Func<Middleware<RequestContext>, Middleware<RequestContext>> middleware)
        {
            _pipelines.Add(middleware);
            return this;
        }

        public Middleware<RequestContext> Build()
        {
            var request = _fallback;
            foreach (var pipeline in _pipelines.Reverse())
            {
                request = pipeline(request);
            }
            return request;
        }
    }
}
