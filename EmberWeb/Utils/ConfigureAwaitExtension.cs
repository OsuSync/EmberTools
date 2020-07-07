using System.Threading.Tasks;

namespace EmberWeb.Utils
{
    public static class ConfigureAwaitExtension
    {
        public static async Task<T> Async<T>(this Task<T> task)
        {
            return await task.ConfigureAwait(false);
        }

        public static async ValueTask<T> Async<T>(this ValueTask<T> task)
        {
            return await task.ConfigureAwait(false);
        }

        public static async Task Async(this Task task)
        {
            await task.ConfigureAwait(false);
        }
    }
}
