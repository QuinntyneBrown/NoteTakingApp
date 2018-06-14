using MediatR;

namespace NoteTakingApp.Core.Identity
{
    public class MaliciousUseDetectedEvent : INotification
    {
        public MaliciousUseDetectedEvent(string username)
            => Username = username;
        
        public string Username { get; set; }
    }
}
