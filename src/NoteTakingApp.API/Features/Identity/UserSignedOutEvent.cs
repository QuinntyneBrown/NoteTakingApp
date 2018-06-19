using NoteTakingApp.API.Hubs;
using NoteTakingApp.Core.Entities;
using NoteTakingApp.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Identity
{
    public class UserSignedOutEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(User user) => User = user;
            public User User { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
 
            private readonly IAccessTokenRepository _repository;

            public Handler(IAccessTokenRepository repository, IHubContext<IntegrationEventsHub> hubContext)
            {
                _repository = repository;
            }                

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {                
                await _repository.InvalidateByUsername(notification.User.Username);                
            }
        }
    }
}
