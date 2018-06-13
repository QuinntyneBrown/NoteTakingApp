using NoteTakingApp.Core.Entities;
using MediatR;

namespace NoteTakingApp.Core.Identity
{
    public class AccessTokenCreatedDomainEvent : INotification
    {
        public AccessTokenCreatedDomainEvent(AccessToken accessToken) => AccessToken = accessToken;
        public AccessToken AccessToken { get; set; }
    }
}
