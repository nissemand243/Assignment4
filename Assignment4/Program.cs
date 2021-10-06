using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Assignment4.Entities;

namespace Assignment4
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var connectionString = "Server=localhost;Database=Kanban;User Id=sa;Password=kode"; //configuration.GetConnectionString("Kanban"); //"Server=localhost;Database=Kanban;User Id=sa;Password=kode"; //illegal 
            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);
        }

        static IConfiguration LoadConfiguration()
{
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Program>();

        return builder.Build();
}
    }
}
