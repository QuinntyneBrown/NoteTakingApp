using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NoteTakingApp.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}
