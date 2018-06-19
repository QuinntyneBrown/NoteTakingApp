using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NoteTakingApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {                
        public static IServiceCollection AddDataStore(this IServiceCollection services,
                                               string connectionString, bool useInMemoryDatabase = false)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();

            if (useInMemoryDatabase) {
                services.AddDbContext<AppDbContext>(options =>
                {
                    options
                    .UseLoggerFactory(AppDbContext.ConsoleLoggerFactory)
                    .UseInMemoryDatabase(databaseName: $"InMemoryDatabase");
                });

                return services;
            }
            services.AddDbContext<AppDbContext>(options =>
            {                
                options
                .UseLoggerFactory(AppDbContext.ConsoleLoggerFactory)
                .UseSqlServer(connectionString, b=> b.MigrationsAssembly("NoteTakingApp.Infrastructure"));
            });

            return services;
        }
    }
}
