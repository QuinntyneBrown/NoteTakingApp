using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoteTakingApp.Core.Identity;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AppHub: Hub
    {
        private IMediator _mediator;

        private static ConcurrentDictionary<string,byte> _connectedUsers = new ConcurrentDictionary<string, byte>();

        public AppHub(IMediator mediator) => _mediator = mediator;
        public string UserName => Context.User.Identity.Name;

        public override async Task OnConnectedAsync()
        {
            if (!_connectedUsers.TryAdd(UserName,0))
            {
                await _mediator.Publish(new MaliciousUseDetectedEvent(UserName));
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
