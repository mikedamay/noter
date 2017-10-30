using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using noter.Services;
using noter.Entities;
using System.Linq;
using noter.ViewModel;

namespace noter.Controllers
{
    public class TagController : Controller
    {
        private TagService _tagService;

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }
        public async Task<IActionResult> index()
        {
            IList<Tag> stuff = await _tagService.ListAll();
            return View(stuff);
        }

        [HttpGet]
        public IActionResult Create()
        {
             return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,ShortDescription,Details")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _tagService.AddAsync(tag);
                return RedirectToAction(nameof(index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long Id)
        {
            var tag = await _tagService.GetById(Id);
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,ShortDescription,Details")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _tagService.UpdateAsync(tag);
                return RedirectToAction(nameof(index));
            }
            return View(tag);
        }
        [HttpGet]
        public async Task<IActionResult> Details(long Id)
        {
            var tag = await _tagService.GetById(Id);
            return View(tag);
        }
        // GET: NoteManager/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _tagService.GetById(id.Value);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: NoteManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _tagService.Delete(id);
            return RedirectToAction(nameof(index));
        }

        private bool NoteExists(long id)
        {
            return _tagService.NoteExists(id);
        }

        public async Task<IActionResult> Select()
        {
            var list = await _tagService.ListAll();
            var tagParts =  list.Select(t => 
              new SelectableTag {Id = t.Id, Name =  t.Name, ShortDescription = t.ShortDescription, Included = false});
            return View(tagParts);
        }
    }
}