using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using noter.Data;
using noter.Entities;
using Microsoft.Extensions.Logging;
using static noter.Common.Utils;

namespace noter.Services
{
    public enum UpdateResult {Success, NoteAlreadyDeleted, ConcurrencyConflict }
    public interface INoteManager
    {
        Task<IList<Note>> ListNotes();
        Task<Note> GetDetails(long? id);
        Task<int> AddNote(Note note);
        Task<UpdateResult> UpdateNote(Note note, IEnumerable<long> tagIds);
        Task<int> DeleteNote(long id);
        Task<Note> GetNoteById(long id);
        bool NoteExists(long id);
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
            int x =await _context.SaveChangesAsync();
//            x =await _context.SaveChangesAsync();
            return x;
        }
        public async Task<UpdateResult> UpdateNote(Note note, IEnumerable<long> tagIds)
        {
//            Assert(note.NoteTags != null);
            if (note.NoteTags == null)
            {
                note.NoteTags = new List<NoteTag>();
            }
            try
            {
                HashSet<long> tagIdSet = tagIds.ToHashSet();

                _context.Database.ExecuteSqlCommand("delete from NoteTag where Noteid = {0}", note.Id);
                
                foreach (NoteTag nt in note.NoteTags)
                {
                    if (tagIdSet.Contains(nt.TagId))
                    {
                        tagIdSet.Remove(nt.TagId);
                    }
                    else  // the existing note has a tag that was not passed in
                    {
                        note.NoteTags.Remove(nt);
                    }
                }
                foreach (long newId in tagIdSet)
                {
                    note.NoteTags.Add(new NoteTag {NoteId = note.Id, TagId = newId});
                }
                _context.Update(note);
                int x = await _context.SaveChangesAsync();
                foreach (NoteTag nnt in note.NoteTags)
                {
                    if (!_context.NoteTag.Any(n => n.NoteId == nnt.NoteId && n.TagId == nnt.TagId))
                    {
                        _context.NoteTag.Add(nnt);
                    }
                }
                x = await _context.SaveChangesAsync();
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
    }
}