using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noter.Data;
using noter.Entities;

namespace noter.Services
{
    public interface INoteManager
    {
        Task<IList<Note>> GetNotes();
        Task<Note> GetDetails(long? id);
        Task<int> Add(Note note);
    }
    public class NoteManager : INoteManager
    {
        private ApplicationDbContext _context;

        public NoteManager(ApplicationDbContext dbContext)
        {
            this._context = dbContext;
        }
        public async Task<IList<Note>> GetNotes()
        {
            await Task.Run(() => Thread.Sleep(1000));
            return new List<Note> {new Note("abc")};
        }

        public async Task<Note> GetDetails(long? id)
        {
            var note = await _context.Note
                .SingleOrDefaultAsync(m => m.Id == id);
            return note;
        }

        public async Task<int> Add(Note note)
        {
            _context.Add(note);
            int x =await _context.SaveChangesAsync();
            return x;
        }
    }
}