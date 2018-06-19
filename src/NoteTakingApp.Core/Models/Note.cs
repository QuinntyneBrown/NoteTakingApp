using System.Collections.Generic;

namespace NoteTakingApp.Core.Entities
{
    public class Note: Entity
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Body { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
    }
}