using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using noter.Services;
namespace noter.Components
{
    public class NoteList : ViewComponent
    {
        private INoteManager _noteManager;
        public NoteList( INoteManager noteManager)
        {
            this._noteManager = noteManager;
           
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _noteManager.ListNotes();
            return View(list);
        }
        
    }
}