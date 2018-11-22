using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using noter.Common;
//using Microsoft.EntityFrameworkCore;
using noter.Entities;
//using noter.Migrations;
using noter.Services;
using noter.ViewModel;
using static noter.Common.Utils;

namespace noter.Controllers
{
    public class NoteManagerController : Controller
    {
        private INoteManager _noteManager;
        private TagService _tagService;

        private static class MyConstants
        {
            public const string AddCommentAction = "Add Comment";
            public const string SaveAction = "Save";
            public const string DeleteCommentAction = "Delete";
            
        }

        public NoteManagerController( INoteManager noteManager, TagService tagService)
        {
            this._noteManager = noteManager;
            this._tagService = tagService;
        }

        // GET: NoteManager
        public async Task<IActionResult> Index()
        {
            ViewBag.SubViewName = "Index";            
            return View("Maintenance", await GetEditNoteVmAsync(null));
        }

        // GET: NoteManager/Create
        public IActionResult Create()
        {
            var sv = GetEditNoteVmAsync(null).Result;
            AddEditSubViewsToViewBag(ViewBag);
            return View("Maintenance", sv);
        }

        // GET: NoteManager/Edit/5 or NoteManager/Edit?id=-1
        /// <summary>
        /// handles create and edit requests
        /// </summary>
        /// <param name="id">-1 == the note is being created, >0 an existing not is being edited</param>
        /// <returns>The multipurpose maintenance view with the "Edit" sub-view
        /// set in ViewBag.SubViewName</returns>
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Note note;
            if (id == Constants.NewEntityId)
            {
                note = new Note();
            }
            else
            {
                note = await _noteManager.GetNoteById(id.Value);
            }
            if (note == null)
            {
                return NotFound();
            }
            AddEditSubViewsToViewBag(ViewBag);
            return View("Maintenance", await GetEditNoteVmAsync(note));
        }

        // POST: NoteManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, long commentId, string submit, EditNoteVM vm)
        {
            if (id != vm.Note.Id)
            {
                return NotFound();
            }   
            if (ModelState.IsValid)
            {
                switch (submit)
                {
                    case MyConstants.AddCommentAction:
                        Assert(commentId == 0);
                        return AddCommentTextBox(vm);
                    case MyConstants.SaveAction:
                        Assert(commentId == 0);
                        return await UpdateNote(vm);
                    case MyConstants.DeleteCommentAction:
                        Assert(commentId != 0);
                        return DeleteComment(vm, commentId);
                }
            }
            AddEditSubViewsToViewBag(ViewBag);
            return View(vm);
        }

        private async Task<IActionResult> UpdateNote(EditNoteVM vm)
        {
            IEnumerable<SelectableTag> selectableTags = vm.SelectableTags;
            UpdateResult result = await _noteManager.UpdateNote(vm.Note
                , selectableTags ?? new List<SelectableTag>()
                , vm.Comments ?? new List<Comment>());
            switch (result)
            {
                case UpdateResult.Success:
                    return RedirectToAction(nameof(Index));
                case UpdateResult.NoteAlreadyDeleted:
                    return NotFound();
                default:
                // case UpdateResult.ConcurrencyConflict:
                    throw new Exception("Another user has edited this record.  Please try again");
            }
        }

        // GET: NoteManager/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _noteManager.GetNoteById(id.Value);
            if (note == null)
            {
                return NotFound();
            }
            ViewBag.SubViewName = "Delete";
            return View("Maintenance", await GetEditNoteVmAsync(note));
        }

        // POST: NoteManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _noteManager.DeleteNote(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AddComment(long Id)
        {
            var note = await _noteManager.GetNoteById(Id);
            if (note == null)
            {
                return NotFound();
            }
            return View(new noter.ViewModel.NoteAndCommentVM{Note = note, Comment = new Comment()});
        }

        [HttpPost]
        public IActionResult AddCommentTextBox(EditNoteVM vm)
        {
            List<Comment> comments = vm.Comments ?? new List<Comment>();
            var comment = new Comment();
            comment.Id = GetTempCommentId(comments);
            comments.Add(comment);
            vm.Comments = comments;
            vm.SelectableTags = vm.SelectableTags ?? new List<SelectableTag>();
            AddEditSubViewsToViewBag(ViewBag);
            return View("Maintenance", vm);
        }
        private IActionResult DeleteComment(EditNoteVM vm, long commentId)
        {
            Assert(vm.Comments != null);
            var newComments = vm.Comments.Where(c => c.Id != commentId).ToList();
            vm.Comments = newComments;
            vm.SelectableTags = vm.SelectableTags ?? new List<SelectableTag>();
            AddEditSubViewsToViewBag(ViewBag);
            return View("Maintenance", vm);
        }

        /// <summary>
        /// returns a negative number being unique (the max + 1) within the comments collection
        /// </summary>
        /// <param name="comments">a list of comments beloning to a note, some persisted, others not</param>
        /// <returns>e.g. -1</returns>
        private long GetTempCommentId(List<Comment> comments)
        {
            long largestId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
            return -(largestId + 1);
        }

        private bool NoteExists(long id)
        {
            return _noteManager.NoteExists(id);
        }

        /// <summary>
        /// creates a model view comprising a note and all the tags 
        /// </summary>
        /// <param name="noteArg">a new or existing note or null (in which case a note will be created</param>
        /// <returns>a view model which contains a note to be edited, all the tags with an indication 
        /// whether each tag is associated with the note</returns>
        private async Task<EditNoteVM> GetEditNoteVmAsync(Note noteArg)
        {
            Note note;
            List<SelectableTag> tagParts;
            HashSet<long> tagIds;
            if (noteArg == null)
            {
                note = new Note{ NoteTags = new List<NoteTag>()};
                tagIds = new HashSet<long>();
            }
            else
            {
                note = noteArg;
                tagIds = note.NoteTags.Select(nt => nt.TagId).ToHashSet();
            }
            var list = await _tagService.ListAll();
            tagParts =  list.Select(t => 
                    new SelectableTag {Id = t.Id, Name =  t.Name, ShortDescription = t.ShortDescription
                        , Included = tagIds.Contains(t.Id)})
                .ToList();
                    
            return new EditNoteVM { Note = note, SelectableTags = tagParts, Comments = note.Comments.ToList()};
        }
        private void AddEditSubViewsToViewBag(object viewBag)
        {
            ViewBag.SubViewName = "Edit";
            ViewBag.AddCommentAction = MyConstants.AddCommentAction;
            ViewBag.SaveAction = MyConstants.SaveAction;
            ViewBag.DeleteAction = MyConstants.DeleteCommentAction;
        }

    }
}

namespace noter.ViewModel
{
    public class NoteAndCommentVM
    {
        public Note Note { get; set; }
        public Comment Comment { get; set; }
    }
}
