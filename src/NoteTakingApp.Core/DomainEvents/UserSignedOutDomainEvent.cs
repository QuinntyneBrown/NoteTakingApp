using MediatR;
using NoteTakingApp.Core.Models;

namespace NoteTakingApp.Core.DomainEvents
{
    public class UserSignedOutDomainEvent: INotification
    {
        public UserSignedOutDomainEvent(User user) => User = user;
        public User User { get; set; }
    }
}
