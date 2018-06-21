using MediatR;

namespace NoteTakingApp.Core.DomainEvents
{
    public class TagRemovedDomainEvent: INotification
    {
        public TagRemovedDomainEvent(int tagId) => TagId = tagId;
        public int TagId { get; set; }
    }
}
