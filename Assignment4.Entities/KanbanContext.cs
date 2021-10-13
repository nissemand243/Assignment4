using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext, IKanbanContext
    {
        public DbSet<Tag> Tags{get; set;}
        public DbSet<User> Users{get; set;}
        public DbSet<Task> Tasks{get; set;}

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }
     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Property(e => e.State).HasConversion(new EnumToStringConverter<State>());
            modelBuilder.Entity<User>().HasIndex(e => e.Email).IsUnique(); 
            modelBuilder.Entity<Tag>().HasIndex(e => e.name).IsUnique(); 
        }

        public int saveChanges()
        {
            return SaveChanges(); 
        }
    }
}
