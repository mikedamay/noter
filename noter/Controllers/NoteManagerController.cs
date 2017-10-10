﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using noter.Data;
using noter.Entities;
using noter.Services;

namespace noter.Controllers
{
    public class NoteManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private INoteManager _noteManager;

        public NoteManagerController(ApplicationDbContext context, INoteManager noteManager)
        {
            _context = context;
            this._noteManager = noteManager;
        }

        // GET: NoteManager
        public async Task<IActionResult> Index()
        {
            //var vv = View(await _context.Note.ToListAsync());
            var list = await _noteManager.GetNotes();
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
                await _noteManager.Add(note);
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

            var note = await _context.Note.SingleOrDefaultAsync(m => m.Id == id);
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
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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

            var note = await _context.Note
                .SingleOrDefaultAsync(m => m.Id == id);
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
            var note = await _context.Note.SingleOrDefaultAsync(m => m.Id == id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(long id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}