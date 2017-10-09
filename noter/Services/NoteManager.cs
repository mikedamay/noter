using noter.Entities;

namespace noter.Services
{
    public interface INoteManager
    {
        Note GetNote();
    }
    public class NoteManager : INoteManager
    {
        public Note GetNote()
        {
            return new Note("abc");
        }
    }
}