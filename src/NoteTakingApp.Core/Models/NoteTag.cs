using NoteTakingApp.Core.Common;

namespace NoteTakingApp.Core.Models
{
    public class NoteTag: Entity
    {        
        public int NoteId { get; set; }
        public int TagId { get; set; }
        public Note Note { get; set; }
        public Tag Tag { get; set; }
    }        
}
