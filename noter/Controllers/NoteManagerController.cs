using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
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
            var list = await _noteManager.ListNotes();
            return View(list);
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
            var vv = View();
            return vv;
        }

        // POST: NoteManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Payload")] Note note)
        {
            if (ModelState.IsValid)
            {
                await _noteManager.AddNote(note);
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: NoteManager/Edit/5
        public async Task<IActionResult> Edit(long? id)
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
                IEnumerable<long> tagIds = vm.SelectableTags.Where(st => st.Included).Select(st => st.Id);
                UpdateResult result = await _noteManager.UpdateNote(vm.Note, tagIds);
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

        private async Task<EditNoteVM> GetEditNoteVmAsync(Note note)
        {
            HashSet<long> tagIds = note.NoteTags.Select(nt => nt.TagId).ToHashSet();
            var list = await _tagService.ListAll();
            var tagParts =  list.Select(t => 
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
