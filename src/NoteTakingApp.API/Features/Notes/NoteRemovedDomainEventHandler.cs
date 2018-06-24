using NoteTakingApp.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using NoteTakingApp.Core;

namespace NoteTakingApp.API.Features.Notes
{
    public class NoteRemovedDomainEventHandler : INotificationHandler<NoteRemovedDomainEvent>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public NoteRemovedDomainEventHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(NoteRemovedDomainEvent @event, CancellationToken cancellationToken)
            => await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Note] Removed",
                Payload = new { @event.NoteId }
            }, cancellationToken);
    }
}
