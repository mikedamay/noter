using System.Collections.Generic;
using noter.Entities;

namespace noter.ViewModel
{
    public class EditNoteVM
    {
        public Note Note { get; set; }
        public IEnumerable<SelectableTag> SelectableTags { get; set; }
    }
}