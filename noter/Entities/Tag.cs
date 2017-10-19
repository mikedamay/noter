using System.ComponentModel.DataAnnotations;

namespace noter.Entities
{
    public class Tag
    {
        public long Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(80)]
        public string ShortDescription { get; set; }
        public string Details { get; set; }
    }
}