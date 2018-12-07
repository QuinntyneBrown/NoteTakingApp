using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NoteTakingApp.Core.Models;
using NoteTakingApp.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
                var context = scope.ServiceProvider.GetService<IAppDbContext>();

                var username = "quinntynebrown@gmail.com";
                var token = _tokenProvider.Issue(username);
                httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");

                var session = context.Sessions.SingleOrDefault(x => x.Username == username);

                if(session == null)
                {
                    context.Sessions.Add(new Session()
                    {
                        AccessToken = token,
                        Username = username,
                        SessionStatus = SessionStatus.LoggedIn
                    });
                    await context.SaveChangesAsync(default(CancellationToken));
                }

                await _next.Invoke(httpContext);
            }
        }
    }
}
