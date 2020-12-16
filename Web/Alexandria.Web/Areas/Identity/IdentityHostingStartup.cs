using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Alexandria.Web.Areas.Identity.IdentityHostingStartup))]

namespace Alexandria.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}
