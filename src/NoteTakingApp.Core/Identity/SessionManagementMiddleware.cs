using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core.Identity
{
    public class SessionManagementMiddleware
    {
        private readonly RequestDelegate _next;
        
        public SessionManagementMiddleware(RequestDelegate next)
            => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            var context = httpContext.RequestServices.GetService<IAppDbContext>();

            var sessions = await context.Sessions
                .Where(x => x.SessionStatus == SessionStatus.Connected)
                .ToListAsync();

            var identity = httpContext.User.Identity;

            if (identity.IsAuthenticated
                && !httpContext.Request.Path.Value.StartsWith("/hub")
                && context.Sessions.SingleOrDefault(x => x.Username == identity.Name && x.SessionStatus == SessionStatus.Connected) == null
                && context.Sessions.SingleOrDefault(x => x.Username == identity.Name && x.SessionStatus == SessionStatus.LoggedIn) == null
                && context.Sessions.SingleOrDefault(x => x.Username == identity.Name && x.SessionStatus == SessionStatus.Disconnected) == null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

                await httpContext.Response.WriteAsync("Unauthorized");
            }
            else if(identity.IsAuthenticated
                && !httpContext.Request.Path.Value.StartsWith("/hub")
                && context.Sessions.SingleOrDefault(x => x.Username == identity.Name && x.SessionStatus == SessionStatus.Disconnected) != null)
            {
                var session = context.Sessions.SingleOrDefault(x => x.Username == identity.Name && x.SessionStatus == SessionStatus.Disconnected);
                session.SessionStatus = SessionStatus.LoggedIn;
                await context.SaveChangesAsync(default(CancellationToken));
            }
            else
                await _next.Invoke(httpContext);
        }
    }
}
