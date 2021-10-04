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
            var connectionString = configuration.GetConnectionString("Kanban");
            var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>().UseSqlServer(connectionString);
            using var context = new KanbanContext(optionsBuilder.Options);
            

        }


        
        static IConfiguration LoadConfiguration()
{
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>();

        return builder.Build();
}
    }
}
