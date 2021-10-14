using System.Collections.Generic;
using Assignment4.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit; 
namespace Assignment4.Entities.Tests
{
    public class UserRepositoryTests
    { 
        private readonly KanbanContext _context; 
        private readonly UserRepository _repo; 

        public UserRepositoryTests()
        {
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
            _repo = new UserRepository(_context);
        }
        [Fact]
        public void Delete_given_user_with_task_and_no_force_returns_conflict()
        {
            var response = _repo.Delete(1);
            Assert.Equal(Response.Conflict, response); 
        }

        [Fact]
        public void Delete_given_user_with_task_and_force_returns_deleted()
        {
            var response = _repo.Delete(1, true);
            Assert.Equal(Response.Deleted, response);
        }

        [Fact]
        public void Delete_given_user_with_task_not_new_or_active_or_resolved_and_no_force_returns_deleted()
        {
            var task1 = new Task {Id =11, Title = "Sleep", State = State.Removed, Tags = new List<Tag> {}};
            var user1 = new User {Id = 5, Name = "Mikkisfar", Email = "yoghurt@yoghurt.dk", Tasks = new List<Task>{}}; 
            var task2 = new Task {Id = 12, Title = "Wake", State = State.Closed, Tags = new List<Tag> {}};
            var user2 = new User {Id = 6, Name = "Mikkismor", Email = "yoggy@yoghurt.dk", Tasks = new List<Task>{}}; 
            _context.Users.AddRange(user1, user2);
            var removedResponse = _repo.Delete(5);
            var closedResponse = _repo.Delete(6);
            Assert.Equal(Response.Deleted, removedResponse);
            Assert.Equal(Response.Deleted, closedResponse);
            
        }

    }
}