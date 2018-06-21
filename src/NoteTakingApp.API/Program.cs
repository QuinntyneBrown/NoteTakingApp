using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoteTakingApp.API.Features;
using NoteTakingApp.API.Features.Identity;
using NoteTakingApp.API.Features.Notes;
using NoteTakingApp.API.Features.Tags;
using NoteTakingApp.Core;
using NoteTakingApp.Core.Behaviours;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Infrastructure.Data;
using NoteTakingApp.Infrastructure.Extensions;
using System;
using System.Linq;

namespace NoteTakingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder().Build();
            ProcessDbCommands(args, host);
            host.Run();
        }
        
        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

        private static void ProcessDbCommands(string[] args, IWebHost host)
        {
            var services = (IServiceScopeFactory)host.Services.GetService(typeof(IServiceScopeFactory));

            using (var scope = services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (args.Contains("ci"))
                    args = new string[4] { "dropdb", "migratedb", "seeddb", "stop" };

                if (args.Contains("dropdb"))
                    context.Database.EnsureDeleted();

                if (args.Contains("migratedb"))
                    context.Database.Migrate();

                if (args.Contains("seeddb"))
                {
                    context.Database.EnsureCreated();
                    SeedData.Seed(context);            
                }
                
                if (args.Contains("stop"))
                    Environment.Exit(0);
            }
        }        
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services.AddDistributedMemoryCache()
                .Configure<AuthenticationSettings>(options => Configuration.GetSection("Authentication").Bind(options))
                .AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"], Configuration.GetValue<bool>("isTest"))
                .AddCustomSecurity(Configuration)
                .AddCustomSignalR()
                .AddCustomSwagger()
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddMediatR(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IAppDbContext context)
        {
            if (Configuration.GetValue<bool>("isTest"))
                app.UseMiddleware<AutoAuthenticationMiddleware>();

            app.UseAuthentication()
                .UseTokenValidation()
                .UseCors("CorsPolicy")
                .UseMvc()
                .UseSignalR(routes => routes.MapHub<IntegrationEventsHub>("/hub"))
                .UseSwagger()
                .UseSwaggerUI(options
                => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Note Taking App API"));            
        }
    }
}
