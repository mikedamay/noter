using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace noter.Entities
{
    public class Tag
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
        [Required]
        [MaxLength(80)]
        [DisplayName("Short Description")]
        public string ShortDescription { get; set; }
        [Required]
        public string Details { get; set; }
        public List<NoteTag> NoteTags { get; set; }
        
        //public virtual ICollection<Note> Notes { get; set; }
    }
}