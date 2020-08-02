using System.Net;

namespace SimpleHttpServer.Host
{
    public class ServerListenerBuilder
    {
        private HttpListener Listener { get; }
        public ServerListenerBuilder()
        {
            Listener = new HttpListener();
        }

        public void Listen(string perfix)
        {
            Listener.Prefixes.Add(perfix);
        }

        public void ListenLocalPort(int port = 80)
        {
            Listener.Prefixes.Add($"http://localhost:{port}/");
            Listener.Prefixes.Add($"http://127.0.0.1:{port}/");
        }

        public ServerListener Build()
        {
            return new ServerListener(Listener);
        }
    }
}
