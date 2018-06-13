using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Identity
{
    public class AutoAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenManager _tokenProvider;

        public AutoAuthenticationMiddleware(ITokenManager tokenProvider, RequestDelegate next) {
            _next = next;
            _tokenProvider = tokenProvider;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var token = _tokenProvider.Issue("quinntynebrown@gmail.com");
            httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
            await _next.Invoke(httpContext);            
        }
    }
}
