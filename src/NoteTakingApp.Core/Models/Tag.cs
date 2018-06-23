using NoteTakingApp.Core.Common;
using NoteTakingApp.Core.Extensions;
using NoteTakingApp.Core.Interfaces;
using System.Collections.Generic;

namespace NoteTakingApp.Core.Models
{
    public class Tag: Entity, IAggregateRoot
    {
        public int TagId { get; set; }
        public string Name { get; private set; }
        public string Slug { get; private set; }
        public ICollection<NoteTag> NoteTags { get; set; } 
            = new HashSet<NoteTag>();

        public void Update(string name)
        {
            Name = name;
            Slug = name.ToSlug();
        }
    }
}
