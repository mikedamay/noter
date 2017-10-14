using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using noter.Entities;
using noter.Services;

namespace noter.Controllers
{
    public class NoteManagerController : Controller
    {
        private INoteManager _noteManager;

        public NoteManagerController( INoteManager noteManager)
        {
            this._noteManager = noteManager;
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
            return View(note);
        }

        // POST: NoteManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Payload")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                UpdateResult result = await _noteManager.UpdateNote(note);
                switch (result)
                {
                    case UpdateResult.Success:
                        return RedirectToAction(nameof(Index));
                    case UpdateResult.NoteAlreadyDeleted:
                        return NotFound();
                    case UpdateResult.ConcurrencyConflict:
                        throw new Exception("Another user has edited this record.  Please try again");
                }
            }
            return View(note);
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

            return View(note);
        }

        // POST: NoteManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _noteManager.DeleteNote(id);
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(long id)
        {
            return _noteManager.NoteExists(id);
        }
    }
}
