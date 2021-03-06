﻿using MediatR;
using Microsoft.AspNetCore.SignalR;
using NoteTakingApp.Core;
using NoteTakingApp.Core.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Identity
{
    public class FailedLoginInvalidUsernameOrPasswordEventHandler : INotificationHandler<FailedLoginInvalidUsernameOrPasswordEvent>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public FailedLoginInvalidUsernameOrPasswordEventHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public Task Handle(FailedLoginInvalidUsernameOrPasswordEvent @event, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
