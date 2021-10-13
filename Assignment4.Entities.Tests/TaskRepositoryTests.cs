using System;
using Assignment4.Core;
using Xunit;
using XUnit.Project.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Data.Sqlite;

namespace Assignment4.Entities.Tests
{
    //[TestCaseOrderer("XUnit.Project.Orderers.PriorityOrderer", "XUnit.Project")]
    public class TaskRepositoryTests
    {   
        [Fact]
        public void Create_returns_created_response_and_id()
        {
            //opret db
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection); 
            var context = new KanbanContext(builder.Options); 
            context.Database.EnsureCreated(); 

            //init repo
            var repo = new TaskRepository(context); 

            var task = new TaskCreateDTO 
            {
                Title = "Java",
                AssignedToId = 5, 
                Description = "Nice coffee",
                Tags = new List<string> {"Important"}
            };

            var response = repo.Create(task); 
            Assert.Equal(Response.Created,response.Response); 
            Assert.Equal(1, response.TaskId); 
        }


        [Fact]
        public void Delete_with_non_existing_entry_returns_notfound()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection); 
            var context = new KanbanContext(builder.Options); 
            context.Database.EnsureCreated();

            var repo = new TaskRepository(context); 
            var deleted = repo.Delete(1);
            Assert.Equal(Response.NotFound, deleted);
        }

        [Fact]
        public void Delete_returns_proper_response_and_removes_task()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection); 
            var context = new KanbanContext(builder.Options); 
            context.Database.EnsureCreated();

            var repo = new TaskRepository(context); 
            //creating a new task just to delete it :)
            var task = new TaskCreateDTO 
            {
                Title = "Java",
                AssignedToId = 5, 
                Description = "Nice coffee",
                Tags = new List<string> {"Important"}
            };
            //check that nothing exists before 
            Assert.Equal(Response.NotFound, repo.Delete(1));
            repo.Create(task);
            //Create method is tested above, so no checking that
            var deleted = repo.Delete(1);
            Assert.Equal(Response.Deleted, deleted);
            //check that the entity actually is removed
            Assert.Equal(Response.NotFound, repo.Delete(1));

            //multiple asserts = bad? 
        }

        [Fact]
        public void Update_successful_returns_proper_response()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection); 
            var context = new KanbanContext(builder.Options); 
            context.Database.EnsureCreated();

            var repo = new TaskRepository(context);
            //create an element to be updated
            var task = new TaskCreateDTO 
            {
                Title = "Java",
                AssignedToId = 5, 
                Description = "Nice coffee",
                Tags = new List<string> {"Important"}
            };
            repo.Create(task);
            
            var taskUpdated = new TaskUpdateDTO 
            {
                Id = 1,
                Title = "CSharp",
                AssignedToId = 5, 
                Description = "Black",
                Tags = new List<string> {"Urgent"}
            };
            var response = repo.Update(taskUpdated);
            Assert.Equal(Response.Updated, response);
            var taskNew = context.Tasks.Find(1);
            Assert.Equal("CSharp", taskNew.Title);
            Assert.Equal("Black", taskNew.Description);
            //mangler evt. de sidste properties
        }

        [Fact]
        public void Update_with_non_existing_returns_not_found()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection); 
            var context = new KanbanContext(builder.Options); 
            context.Database.EnsureCreated();

            var repo = new TaskRepository(context);
            var taskUpdated = new TaskUpdateDTO 
            {
                Id = 1,
                Title = "CSharp",
                AssignedToId = 5, 
                Description = "Black",
                Tags = new List<string> {"Urgent"}
            };
            var response = repo.Update(taskUpdated);
            Assert.Equal(Response.NotFound, response);
        }
    }
}
