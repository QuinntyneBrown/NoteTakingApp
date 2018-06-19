using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Notes
{
    public class NoteRemovedEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(int noteId) => NoteId = noteId;
            public int NoteId { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private readonly IHubContext<IntegrationEventsHub> _hubContext;

            public Handler(IHubContext<IntegrationEventsHub> hubContext)
                => _hubContext = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                await _hubContext.Clients.All.SendAsync("message", new {
                    Type = "[Note] Removed",
                    Payload = new { notification.NoteId }
                }, cancellationToken);
            }
        }
    }
}
