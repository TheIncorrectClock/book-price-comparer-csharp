using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BookSearchAPI
{
    public class BookSearchWebApp
    {
        private static BookSearchWebAppConfiguration config;
        public static void Main(string[] args)
        {
            config = new BookSearchWebAppConfiguration();
            config.LoadNewConfiguration(args[0]);

            var urls = new string[] {
                "http://localhost:" + config.GetValue("port")
            };

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<BookSearchWebApp>()
                .UseUrls(urls)
                .Build();

            host.Run();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(config);
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvcWithDefaultRoute();
        }
    }
}
