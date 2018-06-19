using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Interfaces;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Identity
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenValidationMiddleware(
            RequestDelegate next,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IAccessTokenRepository>();
                var validAccessTokens = await repository.GetValidAccessTokenValuesAsync();
                
                if (httpContext.User.Identity.IsAuthenticated                   
                    && !validAccessTokens.Contains(httpContext.Request.GetAccessToken()))
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await httpContext.Response.WriteAsync("Unauthorized");
                }
                else
                    await _next.Invoke(httpContext);
            }
        }
    }
}
