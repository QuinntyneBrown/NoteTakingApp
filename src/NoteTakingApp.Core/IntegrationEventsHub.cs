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
    public static class ConnectedUsers {
        public static ConcurrentDictionary<string, byte> Value = new ConcurrentDictionary<string, byte>();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IntegrationEventsHub: Hub
    {
        private IAccessTokenRepository _repository;
        
        public IntegrationEventsHub(IAccessTokenRepository repository) {
            _repository = repository;
        }

        public string UserName => Context.User.Identity.Name;

        public override async Task OnConnectedAsync()
        {            
            if (!ConnectedUsers.Value.TryAdd(UserName,0))
            {
                await _repository.InvalidateByUsernameAsync(UserName);
                await _repository.SaveChangesAsync(default(CancellationToken));
                Context.Abort();
            }

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.Value.TryRemove(UserName, out _);
            return base.OnDisconnectedAsync(exception);
        }        
    }    
}
