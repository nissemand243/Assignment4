using System;
using Assignment4.Core;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Data.Sqlite;
using System.Linq;

namespace Assignment4.Entities.Tests
{
    public class TaskRepositoryTests
    {   
        private readonly KanbanContext _context; 
        private readonly TaskRepository _repo; 

        public TaskRepositoryTests()
        {
            //opret db
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<KanbanContext>();
            builder.UseSqlite(connection); 
            var context = new KanbanContext(builder.Options); 
            context.Database.EnsureCreated(); 

             var important = new Tag     { Id = 1, name = "Important" };
            var doesNotMatter = new Tag { Id = 2, name = "Does not matter" };
            var asap = new Tag          { Id = 3, name = "ASAP" };
            var easy = new Tag          { Id = 4, name = "Easy" };
            var hard = new Tag          { Id = 5, name = "Hard" };
            var medium = new Tag        { Id = 6, name = "Medium" };
            var testUser = new User {Id = 25, Name = "Karate Kanninen Karina", Email = "cirkelspark@karateklubben.com"}; //this is only for the id to be used in the java task (bcuz of DTO class)

            var finnishJava = new Task          {Id = 1, Title = "Java", Description = "Flot", AssignedTo = testUser,  State = 0, Tags = new List<Tag> {asap, important, hard}};
            var makeCoffe = new Task            {Id = 2, Title = "Coffee", State = 0, Tags = new List<Tag> {doesNotMatter, medium}};
            var helloWorld = new Task           {Id = 3, Title = "World", State = 0, Tags = new List<Tag> {asap, easy}};
            var beingLate = new Task            {Id = 4, Title = "Late", State = 0, Tags = new List<Tag> {important, doesNotMatter, medium, easy}};
            var makingFun = new Task            {Id = 5, Title = "Fun", State = 0, Tags = new List<Tag> {doesNotMatter, hard}};
            var codingClasses = new Task        {Id = 6, Title = "Classes", State = 0, Tags = new List<Tag> {asap, medium, doesNotMatter}};
            var lookingAtFacebook = new Task    {Id = 7, Title = "Facebook", State = 0, Tags = new List<Tag> {important, doesNotMatter, easy}};
            var cricizingTheExercise = new Task {Id = 8, Title = "Exercise", State = 0, Tags = new List<Tag> {doesNotMatter}};
            var goingToSleep = new Task         {Id = 9, Title = "Sleep", State = 0, Tags = new List<Tag> {hard, easy}};
            var debuggingTheProgram = new Task  {Id = 10, Title = "Program", State = 0, Tags = new List<Tag> {asap, important, doesNotMatter, hard, medium, easy}};

            context.Users.AddRange(
                new User {Id = 1, Name = "Karate Kanninen Karina", Email = "cirkelspark@karateklubben.com", Tasks = new List<Task>{finnishJava, debuggingTheProgram, codingClasses}},
                new User {Id = 2, Name = "Flyvende Bombe Niels", Email = "pilot@911.com", Tasks = new List<Task>{makeCoffe, goingToSleep}},
                new User {Id = 3, Name = "Henning Højspæning", Email = "atomkraftværker@home.com", Tasks = new List<Task>{helloWorld, cricizingTheExercise}},
                new User {Id = 4, Name = "Vibe Vinkelret", Email = "seminekatte@matematiklærer.com", Tasks = new List<Task>{beingLate, lookingAtFacebook, makingFun}}
                );

            context.SaveChanges();


            _context = context; 
            _repo = new TaskRepository(_context);
        }


        [Fact]
        public void Create_returns_created_response_and_id()
        {
            var task = new TaskCreateDTO 
            {
                Title = "Java",
                AssignedToId = 1, 
                Description = "Nice coffee",
                Tags = new List<string> {"Important"}
            };

            var response = _repo.Create(task); 
            Assert.Equal(Response.Created,response.Response); 
            Assert.Equal(11, response.TaskId); 
        }
        

        [Fact]
        public void Create_with_non_existing_user_returns_badrequest()
        {
            //wooow, der skal jo ikke si



        }

        [Fact]
        public void Delete_with_non_existing_entry_returns_notfound()
        {
            var deleted = _repo.Delete(400);
            Assert.Equal(Response.NotFound, deleted);
        }

        [Fact]
        public void Delete_returns_proper_response_and_removes_task()
        {
            //Create method is tested above, so no checking that
            var deleted = _repo.Delete(1);
            Assert.Equal(Response.Deleted, deleted);
            //check that the entity actually is removed
            Assert.Equal(Response.NotFound, _repo.Delete(1));

            //multiple asserts = bad? 
        }

        [Fact]
        public void Update_successful_returns_proper_response()
        {
            //create an element to be updated
            var task = new TaskCreateDTO 
            {
                Title = "Java",
                AssignedToId = 5, 
                Description = "Nice coffee",
                Tags = new List<string> {"Important"}
            };
            _repo.Create(task);
            
            var taskUpdated = new TaskUpdateDTO 
            {
                Id = 1,
                Title = "CSharp",
                AssignedToId = 5, 
                Description = "Black",
                Tags = new List<string> {"Urgent"}
            };
            var response = _repo.Update(taskUpdated);
            Assert.Equal(Response.Updated, response);
            var taskNew = _context.Tasks.Find(1);
            Assert.Equal("CSharp", taskNew.Title);
            Assert.Equal("Black", taskNew.Description);
            //mangler evt. de sidste properties
        }

        [Fact]
        public void Update_with_non_existing_returns_not_found()
        {
            var taskUpdated = new TaskUpdateDTO 
            {
                Id = 1337,
                Title = "CSharp",
                AssignedToId = 5, 
                Description = "Black",
                Tags = new List<string> {"Urgent"}
            };
            var response = _repo.Update(taskUpdated);
            Assert.Equal(Response.NotFound, response);
        }


        [Fact]
        public void Update_given_new_state_updates_stateupdated_timestamp()
        {
            var task = new TaskCreateDTO 
            {
                Title = "Java",
                AssignedToId = 5, 
                Description = "Nice coffee",
                Tags = new List<string> {"Important"}
            };
            _repo.Create(task);
            var oldTime = _context.Tasks.Find(1).StateUpdated; 
            var taskUpdated = new TaskUpdateDTO 
            {
                Id = 1,
                Title = "CSharp",
                AssignedToId = 5, 
                Description = "Black",
                Tags = new List<string> {"Urgent"}
            };
            _repo.Update(taskUpdated); 
            var newTime = _context.Tasks.Find(1).StateUpdated;
            Assert.False(oldTime == newTime);
        }

        [Fact]
        public void Read_returns_correct_task()
        {
            var result = _repo.Read(1); 
            Assert.Equal("Java", result.Title);
            Assert.Equal("Flot", result.Description);
        }

        [Fact]
        public void ReadAll_returns_all_tasks()
        {
            var results = _repo.ReadAll(); 
            var expected = _context.Tasks.Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(ta => ta.name).ToList(), t.State)).ToList();
            //the smart way to think about records  
            for(int i = 0; i< results.Count(); i++)
            {
                Assert.Equal(results.ElementAt(i).ToString(), expected.ElementAt(i).ToString()); 
            }
        }

        [Fact]
        public void ReadAllByState_returns_all_correct_tasks()
        {
            var results = _repo.ReadAllByState(State.New); 
            var expected = _context.Tasks.Select(t => new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, t.Tags.Select(ta => ta.name).ToList(), t.State)).ToList();
            for(int i = 0; i< results.Count(); i++)
            {
                Assert.Equal(results.ElementAt(i).ToString(), expected.ElementAt(i).ToString()); 
            }

        }

        
    }
}
