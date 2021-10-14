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

            var important = new Tag     { name = "Important" };
            var doesNotMatter = new Tag {  name = "Does not matter" };
            var asap = new Tag          { name = "ASAP" };
            var easy = new Tag          {  name = "Easy" };
            var hard = new Tag          { name = "Hard" };
            var medium = new Tag        { name = "Medium" };
            var testUser = new User {Name = "Karate Kanninen Karina", Email = "cirkelspark@karateklubben.com"}; //this is only for the id to be used in the java task (bcuz of DTO class)

            var finnishJava = new Task          {Title = "Java", Description = "Flot", AssignedTo = testUser,  State = 0, Tags = new List<Tag> {asap, important, hard}};
            var makeCoffe = new Task            {Title = "Coffee", State = 0, Tags = new List<Tag> {doesNotMatter, medium}};
            var helloWorld = new Task           {Title = "World", State = 0, Tags = new List<Tag> {asap, easy}};
            var beingLate = new Task            {Title = "Late", State = 0, Tags = new List<Tag> {important, doesNotMatter, medium, easy}};
            var makingFun = new Task            {Title = "Fun", State = 0, Tags = new List<Tag> {doesNotMatter, hard}};
            var codingClasses = new Task        {Title = "Classes", State = 0, Tags = new List<Tag> {asap, medium, doesNotMatter}};
            var lookingAtFacebook = new Task    {Title = "Facebook", State = 0, Tags = new List<Tag> {important, doesNotMatter, easy}};
            var cricizingTheExercise = new Task {Title = "Exercise", State = 0, Tags = new List<Tag> {doesNotMatter}};
            var goingToSleep = new Task         {Title = "Sleep", State = 0, Tags = new List<Tag> {hard, easy}};
            var debuggingTheProgram = new Task  {Title = "Program", State = 0, Tags = new List<Tag> {asap, important, doesNotMatter, hard, medium, easy}};

            context.Users.AddRange(
                new User {Name = "Karate Kanninen Karina", Email = "cirkelspark@karateklubben.com", Tasks = new List<Task>{finnishJava, debuggingTheProgram, codingClasses}},
                new User {Name = "Flyvende Bombe Niels", Email = "pilot@911.com", Tasks = new List<Task>{makeCoffe, goingToSleep}},
                new User { Name = "Henning Højspæning", Email = "atomkraftværker@home.com", Tasks = new List<Task>{helloWorld, cricizingTheExercise}},
                new User {Name = "Vibe Vinkelret", Email = "seminekatte@matematiklærer.com", Tasks = new List<Task>{beingLate, lookingAtFacebook, makingFun}}
                );

            context.SaveChanges();
        }


    }
}

