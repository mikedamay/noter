﻿using System;
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
        Task<UpdateResult> UpdateNote(Note note, IEnumerable<SelectableTag> tagIds, IEnumerable<Comment> comment);
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
            var note = await _context.Note.Include(n => n.NoteTags).Include(n => n.Comments).SingleOrDefaultAsync(m => m.Id == id);
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

        public async Task<UpdateResult> UpdateNote(Note note
          ,IEnumerable<SelectableTag> selectableTags
          ,IEnumerable<Comment> comments)
        {
            var now = DateTime.Now;
            Assert(selectableTags != null);
            Assert(comments != null);
            if (note.NoteTags == null)
            {
                note.NoteTags = new List<NoteTag>();
            }
            try
            {
                _context.Database.ExecuteSqlCommand("delete from NoteTag where Noteid = {0}", note.Id);
                _context.Database.ExecuteSqlCommand("delete from Comments where Noteid = {0}", note.Id);
                var user = new User {Id = 1};
                _context.User.Attach(user);
                note.User = user;
                if (IsNewNote(note))
                {
                    _context.Add(note);
                }
                else
                {
                    _context.Update(note);
                }
                _context.Entry(note).Property(Constants.LastUpdated).CurrentValue = now;
                IncludeSelectedTagsInNote(note, selectableTags);
                foreach (NoteTag ntt in note.NoteTags)
                {
                    _context.NoteTag.Add(ntt);
                    _context.Entry(ntt).Property(Constants.LastUpdated).CurrentValue = now;
                    _context.Entry(ntt).Property(Constants.UserId).CurrentValue = user.Id;
                }
                note.Comments.Clear();
                foreach (var cmt in comments)
                {
                    Comment newComment = new Comment{Payload = cmt.Payload};
                    note.Comments.Add(newComment);
                    _context.CommentSet.Add(newComment );
                    _context.Entry(newComment).Property(Constants.LastUpdated).CurrentValue = now;
                    _context.Entry(newComment).Property(Constants.UserId).CurrentValue = user.Id;
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

        private bool IsNewNote(Note note) => note.Id == Constants.NewEntityId;

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