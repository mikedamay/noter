using System.Collections.Generic;

namespace noter.Entities
{
    public class Note
    {
        public long Id { get; set; }
        public string Payload { get; set; }
        public long? TagId { get; set; }
        public List<NoteTag> NoteTags { get; set; }
        public User User { get; set; }
        public Note()
        {
            
        }
        public Note(string payload)
        {
            Payload = payload;
        }
    }
}
