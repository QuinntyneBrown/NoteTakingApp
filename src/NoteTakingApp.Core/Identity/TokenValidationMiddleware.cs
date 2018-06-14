using Microsoft.AspNetCore.Http;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Identity
{
    public class TokenValidationMiddleware
    {
        private readonly ICache _cache;
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(
            ICache cache,
            RequestDelegate next)
        {
            _cache = cache;
            _next = next;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated 
                && !_cache.Get<List<string>>("ValidAccessTokens").Contains(httpContext.Request.GetAccessToken()))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Unauthorized");                
            }
            else
                await _next.Invoke(httpContext);            
        }
    }
}
