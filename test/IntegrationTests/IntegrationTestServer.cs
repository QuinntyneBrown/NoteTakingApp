using NoteTakingApp.API;
using NoteTakingApp.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public class IntegrationTestServer : TestServer
    {
        public IntegrationTestServer(IWebHostBuilder webHostBuilder)
            : base(webHostBuilder) { }

        public void SeedDatabase()
        {
            var services = (IServiceScopeFactory)Host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                SeedData.Seed(context);
            }
        }
    }
}
