using System.IO;
using Assignment4.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Assignment4.Entities;
using System.Collections.Generic;

namespace Assignment4
{
    public class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
    {
        public KanbanContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Kanban"); //"Server=localhost;Database=Kanban;User Id=sa;Password=4d62252a-acf5-4d84-b663-e2e076d3658c"; 

            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>()
                .UseSqlServer(connectionString);

            return new KanbanContext(optionsBuilder.Options);
        }
    
        public static void Seed(KanbanContext context)
        {
            /* context.Database.ExecuteSqlRaw("DELETE dbo.TagTask"); //Skal ændres til det tabellen hedder
            context.Database.ExecuteSqlRaw("DELETE dbo.Tags");
            context.Database.ExecuteSqlRaw("DELETE dbo.Tasks");
            context.Database.ExecuteSqlRaw("DELETE dbo.Users");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tags', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Tasks', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Users', RESEED, 0)");  */

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
        }


    }
}

