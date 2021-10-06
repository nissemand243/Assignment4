using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Assignment4.Entities
{
    public class KanbanContext : DbContext
    {
        public DbSet<Tag> Tags{get; set;}
        public DbSet<User> Users{get; set;}
        public DbSet<Task> Tasks{get; set;}

        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }
        public KanbanContext() {} 

        //ensure that the enum types are saved as strings in the database 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Property(e => e.State).HasConversion(new EnumToStringConverter<State>());
        }
        
    }
}
