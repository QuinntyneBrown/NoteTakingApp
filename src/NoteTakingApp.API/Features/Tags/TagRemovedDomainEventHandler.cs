using NoteTakingApp.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using NoteTakingApp.Core;

namespace NoteTakingApp.API.Features.Tags
{
    public class TagRemovedDomainEventHandler : INotificationHandler<TagRemovedDomainEvent>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public TagRemovedDomainEventHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(TagRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Tag] Removed",
                Payload = new { notification.TagId }
            }, cancellationToken);
        }
    }
}
