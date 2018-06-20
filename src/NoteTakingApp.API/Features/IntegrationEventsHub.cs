﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NoteTakingApp.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features
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
            Console.WriteLine($"USERNAME: {UserName}");

            if (!_connectedUsers.TryAdd(UserName,0))
            {
                await _repository.InvalidateByUsernameAsync(UserName);
                await _repository.SaveChangesAsync(default(CancellationToken));
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
