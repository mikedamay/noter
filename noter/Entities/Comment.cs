using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace noter.Entities
{
    [Table("Comments")]
    public class Comment
    {
        public long Id { get; set; }
        [DisplayName("Comment Text")]
        public string Payload { get; set; }
    }
}