
using NoteTakingApp.API.Hubs;
using NoteTakingApp.Core;
using NoteTakingApp.Core.Behaviours;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Infrastructure.Data;
using NoteTakingApp.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using static System.Convert;

namespace NoteTakingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthenticationSettings>(options => Configuration.GetSection("Authentication").Bind(options));
            services.AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"]);
            services.AddCustomMvc();
            services.AddCustomSecurity(Configuration);
            services.AddCustomSignalR();                        
            services.AddCustomSwagger();                        
            services.AddMediatR(typeof(Startup));                        
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();
            services.AddSingleton<IAccessTokenRepository, AccessTokenRepository>();
        }
        
        public void Configure(IApplicationBuilder app, IAppDbContext context, ICache cache)
        {
            if (ToBoolean(Configuration["isTest"]))
                app.UseMiddleware<AutoAuthenticationMiddleware>();
                    
            app.UseAuthentication();
            app.UseTokenValidation();
            app.UseCors("CorsPolicy");            
            app.UseMvc();
            app.UseSignalR(routes => routes.MapHub<AppHub>("/hub"));
            app.UseSwagger();
            app.UseSwaggerUI(options 
                => options.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteTakingApp API"));
            
            cache.Add(context.AccessTokens.Where(x => x.ValidTo > DateTime.UtcNow).Select(x => x.Value).ToList(), "ValidAccessTokens");
        }        
    }
}
