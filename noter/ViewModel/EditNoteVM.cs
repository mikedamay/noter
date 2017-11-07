using System.Collections.Generic;
using noter.Entities;

namespace noter.ViewModel
{
    public class EditNoteVM
    {
        public Note Note { get; set; }
        public List<SelectableTag> SelectableTags { get; set; }
        public bool Checker { get; set; }
        public Comment Comment { get; set; }
    }
}