using System;
using System.Collections.Generic;
using Assignment4.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assignment4.Core
{
    public interface IKanbanContext : IDisposable
    {
        public DbSet<Tag> Tags{get; set;}
        public DbSet<User> Users{get; set;}
        public DbSet<Task> Tasks{get; set;}
        int saveChanges(); 
    }
}
