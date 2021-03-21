using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GeoSearch.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel((context, options) =>
                {
                    options.Limits.MaxRequestBodySize = null;
                })
                .UseStartup<Startup>();
            });
    }
}
