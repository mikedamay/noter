using System.Collections.Generic;

namespace noter.Entities
{
    public class Note
    {
        public long Id { get; set; }
        public string Payload { get; set; }
        public HashSet<Tag> Tags { get; set; }
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
