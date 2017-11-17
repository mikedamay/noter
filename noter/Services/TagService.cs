using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noter.Data;
using noter.Entities;
using noter.Common;
using System;

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
            var user = new User {Id = 1};
            _dbContext.Tag.Add(tag);
            _dbContext.Entry(tag).Property(Constants.LastUpdated).CurrentValue = DateTime.Now;
            _dbContext.Entry(tag).Property(Constants.UserId).CurrentValue = user.Id;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            var user = new User {Id = 1};
            _dbContext.Tag.Update(tag);
            _dbContext.Entry(tag).Property(Constants.LastUpdated).CurrentValue = DateTime.Now;
            _dbContext.Entry(tag).Property(Constants.UserId).CurrentValue = user.Id;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var tag = await _dbContext.Tag.SingleAsync(t => t.Id == id);
            _dbContext.Tag.Remove(tag);
            await _dbContext.SaveChangesAsync();
        }

        public bool NoteExists(long id)
        {
            return _dbContext.Tag.Any(t => t.Id == id);
        }
    }
}