using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(EmberWeb.Areas.Identity.IdentityHostingStartup))]
namespace EmberWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
