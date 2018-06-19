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
            private readonly ICache _cache;
            private readonly IAccessTokenRepository _repository;

            public Handler(IAccessTokenRepository repository, IHubContext<IntegrationEventsHub> hubContext, ICache cache)
            {
                _cache = cache;
                _repository = repository;
            }                

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                foreach (var accessToken in _repository.GetValidTokensByUsername(notification.User.Username))
                {
                    accessToken.ValidTo = DateTime.UtcNow.AddYears(-1);
                }

                await _repository.SaveChangesAsync(cancellationToken);
                
                _cache.Add<List<string>>(_repository.GetValidAccessTokenValues(), "ValidAccessTokens");
                
            }
        }
    }
}
