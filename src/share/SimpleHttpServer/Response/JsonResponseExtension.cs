using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleHttpServer.Response
{
    public static class JsonResponseExtension
    {
        public static ValueTask Send(this HttpListenerResponse res, int statusCode = 200, object content = null, bool emptyResponse = false)
        {
            res.StatusCode = statusCode;
            res.ContentType = "application/json";
            if (emptyResponse)
            {
                res.Close();
                return default;
            }
            var resContent = JsonSerializer.SerializeToUtf8Bytes(content);
            res.ContentEncoding = Encoding.UTF8;
            res.ContentLength64 = resContent.Length;
            res.Close(resContent, false);
            return default;
        }
        public static ValueTask Ok(this HttpListenerResponse res, object content = null, bool emptyResponse = false)
        {
            return Send(res, 200, content, emptyResponse);
        }

        public static ValueTask NotFound(this HttpListenerResponse res)
        {
            return Send(res, 404, emptyResponse: true);
        }
    }
}
