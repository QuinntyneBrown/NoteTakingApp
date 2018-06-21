using MediatR;

namespace NoteTakingApp.Core.DomainEvents
{
    public class NoteRemovedDomainEvent: INotification
    {
        public NoteRemovedDomainEvent(int noteId) => NoteId = noteId;
        public int NoteId { get; set; }
    }
}
