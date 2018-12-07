using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoteTakingApp.Core.Interfaces;
using NoteTakingApp.Core.Models;
using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IntegrationEventsHub: Hub
    {
        private IAppDbContext _context;
        
        public IntegrationEventsHub(IAppDbContext context) 
            => _context = context;

        private static ConcurrentDictionary<string, byte> _connectedUsers = new ConcurrentDictionary<string, byte>();

        public string UserName => (Context.User.Identity as ClaimsIdentity).Claims.Single(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
        
        public override async Task OnConnectedAsync()
        {
            if (!_connectedUsers.TryAdd(UserName, 0))
            {
                var session = _context.Sessions.Single(x => x.Username == UserName && x.SessionStatus == SessionStatus.Connected);
                session.SessionStatus = SessionStatus.Invalid;
            }
            else {
                var session = _context.Sessions
                    .Single(x => x.Username == UserName && (x.SessionStatus == SessionStatus.LoggedIn || x.SessionStatus == SessionStatus.Connected));
                session.SessionStatus = SessionStatus.Connected;                
            }

            await _context.SaveChangesAsync(default(CancellationToken));

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectedUsers.TryRemove(UserName, out _);
            
            await base.OnDisconnectedAsync(exception);
        }        
    }    
}
