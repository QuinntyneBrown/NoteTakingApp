using NoteTakingApp.Core.Common;
using NoteTakingApp.Core.Interfaces;
using System.Collections.Generic;

namespace NoteTakingApp.Core.Models
{
    public class Note: Entity, IAggregateRoot
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
    }
}
