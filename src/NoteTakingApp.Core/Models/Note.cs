using NoteTakingApp.Core.Common;
using NoteTakingApp.Core.Extensions;
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

        public void Update(string title, string body, ICollection<Tag> tags) {
            Body = body;
            Title = title;
            Slug = title.ToSlug();
            NoteTags.Clear();
            foreach(var tag in tags)
                NoteTags.Add(new NoteTag() { Tag = tag });
        }
    }
}
