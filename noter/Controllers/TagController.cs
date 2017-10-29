using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;
using noter.Services;
using noter.Entities;

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
        public async Task<IActionResult> Details(long Id)
        {
            var tag = await _tagService.GetById(Id);
            return View(tag);
        }

    }
}