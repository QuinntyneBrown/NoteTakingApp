using NoteTakingApp.Core.DomainEvents;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using NoteTakingApp.Core;

namespace NoteTakingApp.API.Features.Tags
{
    public class TagSavedDomainEventHandler : INotificationHandler<TagSavedDomainEvent>
    {
        private readonly IHubContext<IntegrationEventsHub> _hubContext;

        public TagSavedDomainEventHandler(IHubContext<IntegrationEventsHub> hubContext)
            => _hubContext = hubContext;

        public async Task Handle(TagSavedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.All.SendAsync("message", new
            {
                Type = "[Tag] Saved",
                Payload = new { Tag = TagApiModel.FromTag(notification.Tag) }
            }, cancellationToken);
        }
    }
}
