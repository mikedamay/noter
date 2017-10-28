using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            
    }
}