using NoteTakingApp.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteTakingApp.Core.Models
{
    public class NoteTag
    {        
        public int NoteId { get; set; }
        public int TagId { get; set; }
        public Note Note { get; set; }
        public Tag Tag { get; set; }
    }        
}
