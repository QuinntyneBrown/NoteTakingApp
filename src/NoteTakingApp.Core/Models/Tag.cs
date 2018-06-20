using NoteTakingApp.Core.Interfaces;
using System.Collections.Generic;

namespace NoteTakingApp.Core.Models
{
    public class Tag: Entity, IAggregateRoot
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; } = new HashSet<NoteTag>();
    }
}
