using NoteTakingApp.Core.Models;
using MediatR;

namespace NoteTakingApp.Core.DomainEvents
{
    public class NoteSavedDomainEvent: INotification
    {
        public NoteSavedDomainEvent(Note note) => Note = note;
        public Note Note { get; set; }
    }
}
