using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
            EntityTypeBuilder<NoteTag> etb_nt = mb.Entity<NoteTag>();
            KeyBuilder kb_nt = etb_nt.HasKey(nt => new {nt.NoteId, nt.TagId});
            ReferenceNavigationBuilder<NoteTag, Note> rnb_ntn = etb_nt.HasOne<Note>(nt => nt.Note);
            ReferenceCollectionBuilder<Note, NoteTag> rcb_ntn = rnb_ntn.WithMany();
            ReferenceCollectionBuilder<Note, NoteTag> rcb_ntn2 = rcb_ntn.HasForeignKey(nt => nt.NoteId);
            ReferenceNavigationBuilder<NoteTag, Tag> rnb_ntt = etb_nt.HasOne(nt => nt.Tag);
            ReferenceCollectionBuilder<Tag, NoteTag> rcb_ntt = rnb_ntt.WithMany(t => t.NoteTags);
            ReferenceCollectionBuilder<Tag, NoteTag> rcb_ntt2 = rcb_ntt.HasForeignKey(nt => nt.TagId);
            mb.Entity<Note>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<Tag>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<Comment>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<NoteTag>().Property<DateTime>(Constants.LastUpdated);
            mb.Entity<Tag>().Property<long>(Constants.UserId);
            mb.Entity<Comment>().Property<long>(Constants.UserId);
            mb.Entity<NoteTag>().Property<long>(Constants.UserId);
            mb.Entity<Tag>().HasOne<User>().WithMany().HasForeignKey(nameof(Constants.UserId)).OnDelete(DeleteBehavior.Restrict);
            mb.Entity<Comment>().HasOne<User>().WithMany().HasForeignKey(Constants.UserId).OnDelete(DeleteBehavior.Restrict);
            mb.Entity<NoteTag>().HasOne<User>().WithMany().HasForeignKey(Constants.UserId).OnDelete(DeleteBehavior.Restrict);
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
