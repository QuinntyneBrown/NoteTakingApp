using Microsoft.AspNetCore.Builder;
using NoteTakingApp.Core.Identity;

namespace NoteTakingApp.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder app)
            => app.UseMiddleware<TokenValidationMiddleware>();
    }
}