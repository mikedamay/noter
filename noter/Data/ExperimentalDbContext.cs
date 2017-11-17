using Microsoft.EntityFrameworkCore;
using noter.Entities;
using noter.Entities.Experimental;

namespace noter.Data
{
    public class ExperimentalDbContext : DbContext
    {
        public ExperimentalDbContext(DbContextOptions<ExperimentalDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Child>().HasOne(c => c.Parent).WithMany().HasForeignKey("ParentId")
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
    }
}