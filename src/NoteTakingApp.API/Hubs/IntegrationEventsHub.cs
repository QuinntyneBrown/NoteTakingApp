using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoteTakingApp.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class IntegrationEventsHub: Hub
    {
        private IAccessTokenRepository _repository;

        private static ConcurrentDictionary<string,byte> _connectedUsers = new ConcurrentDictionary<string, byte>();

        public IntegrationEventsHub(IAccessTokenRepository repository) => _repository = repository;
        public string UserName => Context.User.Identity.Name;

        public override async Task OnConnectedAsync()
        {
            if (!_connectedUsers.TryAdd(UserName,0))
            {
                await _repository.InvalidateByUsername(UserName);
                Context.Abort();
            }

            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _connectedUsers.TryRemove(UserName, out _);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send(object message) => await Clients.All.SendAsync("message", message);
    }
}
