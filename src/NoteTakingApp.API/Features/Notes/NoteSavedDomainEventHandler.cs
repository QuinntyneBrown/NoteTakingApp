using NoteTakingApp.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using NoteTakingApp.Core;

namespace NoteTakingApp.API.Features.Notes
{
    public class NoteSavedDomainEventHandler : INotificationHandler<NoteSavedDomainEvent>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public NoteSavedDomainEventHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(NoteSavedDomainEvent @event, CancellationToken cancellationToken)
            => await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Note] Saved",
                Payload = new { Note = NoteApiModel.FromNote(@event.Note) }
            }, cancellationToken);
    }
}
