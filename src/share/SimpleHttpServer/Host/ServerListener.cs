using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleHttpServer.Host
{
    public class ServerListener : IDisposable
    {
        public HttpListener Listener { get; set; }
        public ServerListener(HttpListener httpListener)
        {
            Listener = httpListener;
            Listener.Start();
        }

        public event Func<HttpListenerContext, CancellationToken, ValueTask> ProcessRequest;
        public async ValueTask HandleRequest(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ProcessRequest?.Invoke(await Listener.GetContextAsync(), cancellationToken);
            }
        }

        public void Dispose()
        {
            Listener.Stop();
        }
    }
}
