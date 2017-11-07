using System.Collections.Generic;
using System.ComponentModel;

namespace noter.Entities
{
    public class Note
    {
        public long Id { get; set; }
        [DisplayName("Note Text")]
        public string Payload { get; set; }
        public List<NoteTag> NoteTags { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Note()
        {
            this.Comments = new HashSet<Comment>();
            this.NoteTags = new List<NoteTag>();
            this.User = new User();
        }
    }
}
