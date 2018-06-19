using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Identity
{
    public class AutoAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenManager _tokenProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        
        public AutoAuthenticationMiddleware(
            ITokenManager tokenProvider, 
            RequestDelegate next, 
            IServiceScopeFactory serviceScopeFactory) {            
            _next = next;
            _tokenProvider = tokenProvider;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetService<IAccessTokenRepository>();
                var username = "quinntynebrown@gmail.com";
                var token = _tokenProvider.Issue(username);
                httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
                repository.Add(new AccessToken() {
                    Value = token,
                    Username = username,
                    ValidTo = _tokenProvider.GetValidToDateTime(token)
                });
                await repository.SaveChangesAsync(default(CancellationToken));                
                await _next.Invoke(httpContext);
            }
        }
    }
}
