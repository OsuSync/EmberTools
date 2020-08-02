using System.Net;
using System.Threading;

namespace SimpleHttpServer.Pipeline
{
    public struct RequestContext
    {
        public HttpListenerContext Http { get; set; }
        public CancellationToken CancelToken { get; set; }
    }
}
