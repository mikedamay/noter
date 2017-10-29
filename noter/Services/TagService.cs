using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noter.Data;
using noter.Entities;

namespace noter.Services
{
    public class TagService
    {
        private NoteDbContext _dbContext;

        public TagService(NoteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Tag>> ListAll()
        {
            var list = await _dbContext.Tag.ToListAsync();
            return list;
        }

        public async Task<Tag> GetById(long id)
        {
            var tag = await _dbContext.Tag.SingleAsync(t => t.Id == id);
            return tag;
        }

        public async Task AddAsync(Tag tag)
        {
            _dbContext.Tag.Add(tag);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}