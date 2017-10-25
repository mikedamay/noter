using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noter.Entities;

namespace noter.Data
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<NoteTag>().HasKey(nt => new {nt.NoteId, nt.TagId});
            mb.Entity<NoteTag>().HasOne(nt => nt.Note).WithMany(n => n.NoteTags).HasForeignKey(nt => nt.NoteId);
            mb.Entity<NoteTag>().HasOne(nt => nt.Tag).WithMany(t => t.NoteTags).HasForeignKey(nt => nt.TagId);
        }

        public DbSet<noter.Entities.Note> Note { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> User { get; set; }
    }
}
