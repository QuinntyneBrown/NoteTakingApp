using NoteTakingApp.Core.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace NoteTakingApp.API.Features.Notes
{
    public class NoteSavedEvent
    {
        public class DomainEvent : INotification
        {
            public DomainEvent(Note note) => Note = note;
            public Note Note { get; set; }
        }

        public class Handler : INotificationHandler<DomainEvent>
        {
            private readonly IHubContext<IntegrationEventsHub> _context;

            public Handler(IHubContext<IntegrationEventsHub> hubContext)
                => _context = hubContext;

            public async Task Handle(DomainEvent notification, CancellationToken cancellationToken) {
                await _context.Clients.All.SendAsync("message", new {
                    Type = "[Note] Saved",
                    Payload = new { Note = NoteApiModel.FromNote(notification.Note) }
                }, cancellationToken);
            }
        }
    }
}
