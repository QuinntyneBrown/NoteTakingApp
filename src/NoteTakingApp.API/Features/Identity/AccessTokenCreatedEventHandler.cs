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
        private readonly IAccessTokenRepository _repository;
        public AccessTokenCreatedEventHandler(IAccessTokenRepository repository, IAppDbContext context, ICache cache)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task Handle(AccessTokenCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _cache.Add(_repository.GetValidAccessTokenValues(), "ValidAccessTokens");            
            await Task.CompletedTask;
        }
    }
}
