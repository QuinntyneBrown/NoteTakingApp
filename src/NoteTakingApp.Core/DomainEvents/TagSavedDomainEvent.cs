using NoteTakingApp.Core.Models;
using MediatR;

namespace NoteTakingApp.Core.DomainEvents
{
    public class TagSavedDomainEvent: INotification
    {
        public TagSavedDomainEvent(Tag tag) => Tag = tag;
        public Tag Tag { get; set; }
    }
}
