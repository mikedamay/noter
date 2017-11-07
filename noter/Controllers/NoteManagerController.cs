using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using noter.Common;
//using Microsoft.EntityFrameworkCore;
using noter.Entities;
using noter.Services;
using noter.ViewModel;

namespace noter.Controllers
{
    public class NoteManagerController : Controller
    {
        private INoteManager _noteManager;
        private TagService _tagService;

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

        // GET: NoteManager/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Note note = await _noteManager.GetDetails(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // GET: NoteManager/Create
        public IActionResult Create()
        {
            var sv = GetEditNoteVmAsync(null).Result;
            ViewBag.SubViewName = "Edit";
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
            ViewBag.SubViewName = "Edit";
            return View("Maintenance", await GetEditNoteVmAsync(note));
        }

        // POST: NoteManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, EditNoteVM vm)
        {
            if (id != vm.Note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                IEnumerable<SelectableTag> selectableTags = vm.SelectableTags;
                UpdateResult result = await _noteManager.UpdateNote(vm.Note, selectableTags);
                switch (result)
                {
                    case UpdateResult.Success:
//                        return View(new EditNoteVM {Note = vm.Note, SelectableTags = vm.SelectableTags});
                        return RedirectToAction(nameof(Index));
                    case UpdateResult.NoteAlreadyDeleted:
                        return NotFound();
                    case UpdateResult.ConcurrencyConflict:
                        throw new Exception("Another user has edited this record.  Please try again");
                }
            }
            ViewBag.SubViewName = "Edit";
            return View(vm);
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
        public async Task<IActionResult> AddComment(NoteAndCommentVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.Note.Comments.Add(vm.Comment);
                await _noteManager.AddComment(vm.Note);
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
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
                    
            return new EditNoteVM { Note = note, SelectableTags = tagParts};
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
