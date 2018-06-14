using NoteTakingApp.Core.Identity;
using NoteTakingApp.Core.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace NoteTakingApp.API.Features.Identity
{
    public class MaliciousUseDetectedEventHandler: INotificationHandler<MaliciousUseDetectedEvent>
    {
        private readonly ICache _cache;
        private readonly IAccessTokenRepository _repository;

        public MaliciousUseDetectedEventHandler(ICache cache, IAccessTokenRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public async Task Handle(MaliciousUseDetectedEvent notification, CancellationToken cancellationToken)
        {
            foreach (var accessToken in _repository.GetByUsername(notification.Username))
            {
                accessToken.ValidTo = default(DateTime);
            }

            await _repository.SaveChangesAsync(cancellationToken);
            
            _cache.Remove("ValidAccessTokens");
        }
    }
}
