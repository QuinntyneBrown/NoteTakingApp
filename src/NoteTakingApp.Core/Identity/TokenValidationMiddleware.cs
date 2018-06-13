using NoteTakingApp.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Identity
{
    public class TokenValidationMiddleware
    {
        private readonly ICache _cache;
        private readonly IAppDbContext _context;
        private readonly RequestDelegate _next;
        private readonly ITokenManager _tokenProvider;
        public TokenValidationMiddleware(
            IServiceScopeFactory serviceScopeFactory,
            ICache cache,
            ITokenManager tokenProvider, 
            RequestDelegate next)
        {
            _cache = cache;
            _next = next;
            _tokenProvider = tokenProvider;
        }
        
        public async Task Invoke(HttpContext httpContext)
        {           
            if (httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);

                if(value == default(StringValues))
                    value = httpContext.Request.Query["token"];

                var validTokens = _cache.Get<List<string>>("ValidAccessTokens");

                var accessToken = value.ToString().Replace("Bearer ", "");

                if (validTokens == null || validTokens.FirstOrDefault(x => x == accessToken) == null)
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Unauthorized");
                }
                else
                {
                    await _next.Invoke(httpContext);
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }
        }
    }
}
