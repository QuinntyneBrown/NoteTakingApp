using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
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
            if (httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);

                if(StringValues.IsNullOrEmpty(value)) value = httpContext.Request.Query["token"];

                var validTokens = _cache.Get<List<string>>("ValidAccessTokens");

                var accessToken = value.ToString().Replace("Bearer ","");

                if (!validTokens.Contains(accessToken))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized");
                }
                else
                    await _next.Invoke(httpContext);
            }
            else
                await _next.Invoke(httpContext);            
        }
    }
}
