﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noter.Data;
using noter.Entities;

namespace noter.Services
{
    public enum UpdateResult {Success, NoteAlreadyDeleted, ConcurrencyConflict }
    public interface INoteManager
    {
        Task<IList<Note>> ListNotes();
        Task<Note> GetDetails(long? id);
        Task<int> AddNote(Note note);
        Task<UpdateResult> UpdateNote(Note note);
        Task<int> DeleteNote(long id);
        Task<Note> GetNoteById(long id);
        bool NoteExists(long id);
    }
    public class NoteManager : INoteManager
    {
        private NoteDbContext _context;

        public NoteManager(NoteDbContext dbContext)
        {
            this._context = dbContext;
        }
        public async Task<IList<Note>> ListNotes()
        {
            var list = await _context.Note.ToListAsync();
            return list;
        }

        public async Task<Note> GetNoteById(long id)
        {
            var note = await _context.Note.SingleOrDefaultAsync(m => m.Id == id);
            _context.Note.ToList();
            return note;
        }
        public async Task<Note> GetDetails(long? id)
        {
            var note = await _context.Note
                .SingleOrDefaultAsync(m => m.Id == id);
            return note;
        }

        public async Task<int> AddNote(Note note)
        {
            _context.Add(note);
            int x =await _context.SaveChangesAsync();
            return x;
        }
        public async Task<UpdateResult> UpdateNote(Note note)
        {
            try
            {
                _context.Update(note);
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