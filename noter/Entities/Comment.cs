using System.ComponentModel;

namespace noter.Entities
{
    public class Comment
    {
        public long Id { get; set; }
        [DisplayName("Comment Text")]
        public string Payload { get; set; }
    }
}