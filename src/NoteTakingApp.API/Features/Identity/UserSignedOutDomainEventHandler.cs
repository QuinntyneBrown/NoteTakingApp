using NoteTakingApp.Core.Models;
using NoteTakingApp.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using NoteTakingApp.Core;
using NoteTakingApp.Core.DomainEvents;

namespace NoteTakingApp.API.Features.Identity
{
    public class UserSignedOutDomainEventHandler : INotificationHandler<UserSignedOutDomainEvent>
    {
        private readonly ISessionRepository _repository;

        public UserSignedOutDomainEventHandler(ISessionRepository repository, IHubContext<IntegrationEventsHub> hubContext)
                => _repository = repository;

        public async Task Handle(UserSignedOutDomainEvent notification, CancellationToken cancellationToken)
        {
            await _repository.InvalidateByUsernameAsync(notification.User.Username);
            await _repository.SaveChangesAsync(cancellationToken);
        }
    }

}
