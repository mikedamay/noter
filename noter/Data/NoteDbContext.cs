using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using noter.Entities;
using noter.Common;

namespace noter.Data
{
    public class NoteDbContext : DbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options) : base(options)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<NoteTag>().HasKey(nt => new {nt.NoteId, nt.TagId});
            mb.Entity<NoteTag>().HasOne(nt => nt.Note).WithMany(n => n.NoteTags).HasForeignKey(nt => nt.NoteId);
            mb.Entity<NoteTag>().HasOne(nt => nt.Tag).WithMany(t => t.NoteTags).HasForeignKey(nt => nt.TagId);
            mb.Entity<Note>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<Tag>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<Comment>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<NoteTag>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<Comment>().HasOne<Note>().WithMany(n => n.Comments).HasForeignKey("NoteId")
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<noter.Entities.Note> Note { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<NoteTag> NoteTag { get; set; }
        public DbSet<Another> Another { get; set; }
        public DbSet<Comment> CommentSet { get; set; }
    }
}
