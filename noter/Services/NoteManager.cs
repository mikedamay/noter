using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using noter.Data;
using noter.Entities;
using Microsoft.Extensions.Logging;
using noter.ViewModel;
using static noter.Common.Utils;
using noter.Common;

namespace noter.Services
{
    public enum UpdateResult {Success, NoteAlreadyDeleted, ConcurrencyConflict }
    public interface INoteManager
    {
        Task<IList<Note>> ListNotes();
        Task<Note> GetDetails(long? id);
        Task<int> AddNote(Note note);
        Task<UpdateResult> UpdateNote(Note note, IEnumerable<SelectableTag> tagIds);
        Task<int> DeleteNote(long id);
        Task<Note> GetNoteById(long id);
        bool NoteExists(long id);
        Task<int> AddComment(Note vmNote);
    }

    public class NoteManager : INoteManager
    {
        private NoteDbContext _context;
        private ILogger<NoteManager> _logger;

        public NoteManager(NoteDbContext dbContext, ILogger<NoteManager> logger)
        {
            this._logger = logger;
            this._context = dbContext;
            _logger.LogInformation(1, "created NoteManager");
        }

        public async Task<IList<Note>> ListNotes()
        {
            var list = await _context.Note.ToListAsync();
            return list;
        }

        public async Task<Note> GetNoteById(long id)
        {
            var note = await _context.Note.Include(n => n.NoteTags).SingleOrDefaultAsync(m => m.Id == id);
            var ll = _context.Note.ToList();
            return note;
        }

        public async Task<Note> GetDetails(long? id)
        {
            var note = await _context.Note
//                .Include(n => n.NoteTags)
                .SingleOrDefaultAsync(m => m.Id == id);
            return note;
        }

        public async Task<int> AddNote(Note note)
        {
            _context.Add(note);
            _logger.LogDebug(1234, "about to write NoteTag");
            var nt = new NoteTag {NoteId = note.Id, TagId = 2};
            _context.Add(nt);
            int x = await _context.SaveChangesAsync();
//            x =await _context.SaveChangesAsync();
            return x;
        }

        public async Task<UpdateResult> UpdateNote(Note note, IEnumerable<SelectableTag> selectableTags)
        {
            if (note.NoteTags == null)
            {
                note.NoteTags = new List<NoteTag>();
            }
            try
            {
                _context.Database.ExecuteSqlCommand("delete from NoteTag where Noteid = {0}", note.Id);

                if (note.Id == Constants.NewEntityId)
                {
                    _context.Add(note);
                }
                else
                {
                    _context.Update(note);
                }
                IncludeSelectedTagsInNote(note, selectableTags);
                foreach (NoteTag ntt in note.NoteTags)
                {
                    _context.NoteTag.Add(ntt);
                }
                int x = await _context.SaveChangesAsync();
                return UpdateResult.Success;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(note.Id))
                {
                    return UpdateResult.NoteAlreadyDeleted;
                }
                else
                {
                    return UpdateResult.ConcurrencyConflict;
                }
            }
        }

        private void IncludeSelectedTagsInNote(Note note, IEnumerable<SelectableTag> selectableTags)
        {
            HashSet<long> tagIdSetToInclude = selectableTags.Where(st => st.Included).Select(st => st.Id).ToHashSet();
            note.NoteTags.Clear();
            foreach (long selectedTagId in tagIdSetToInclude)
            {
                note.NoteTags.Add(new NoteTag {NoteId = note.Id, TagId = selectedTagId});
            }
            
        }

    public async Task<int> DeleteNote(long id)
        {
            try
            {
                var note = await _context.Note.SingleOrDefaultAsync(m => m.Id == id);
                _context.Note.Remove(note);
                int x = await _context.SaveChangesAsync();
                return x;

            }
            catch (System.Exception ex)
            {

                throw ex;
            }        
        }

        public bool NoteExists(long id)
        {
            return _context.Note.Any(e => e.Id == id);
        }

        public async Task<int> AddComment(Note note)
        {
            _context.CommentSet.AddRange(note.Comments);
            foreach (Comment c in note.Comments)
            {
                _context.Entry(c).Property("NoteId").CurrentValue = note.Id;
            }
            int x = await _context.SaveChangesAsync();
            return x;
        }
    }
}