using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Identity
{
    public class AccessTokenCreatedEventHandler : INotificationHandler<AccessTokenCreatedDomainEvent>
    {
        private readonly ICache _cache;
        public AccessTokenCreatedEventHandler(ICache cache)
            => _cache = cache;

        public async Task Handle(AccessTokenCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _cache.Remove("ValidAccessTokens");            
            await Task.CompletedTask;
        }
    }
}
