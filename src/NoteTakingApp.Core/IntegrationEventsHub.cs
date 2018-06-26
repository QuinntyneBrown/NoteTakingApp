using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoteTakingApp.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.Core
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IntegrationEventsHub: Hub
    {
        private IAccessTokenRepository _repository;
        
        public IntegrationEventsHub(IAccessTokenRepository repository) 
            => _repository = repository;

        private static ConcurrentDictionary<string, byte> _connectedUsers = new ConcurrentDictionary<string, byte>();

        public string UserName => Context.User.Identity.Name;
        
        public override async Task OnConnectedAsync()
        {            
            if (!_connectedUsers.TryAdd(UserName, 0))
            {
                await _repository.InvalidateByUsernameAsync(UserName);
                await _repository.SaveChangesAsync(default(CancellationToken));
            }
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectedUsers.TryRemove(UserName, out _);
            await base.OnDisconnectedAsync(exception);
        }        
    }    
}
