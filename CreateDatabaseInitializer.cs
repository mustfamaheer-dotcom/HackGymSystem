using System;
using Microsoft.EntityFrameworkCore;
using Gym.Infrastructure.Data;

public class InitializeDatabase
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Initializing SQLite database for Gym API...");
        
        var optionsBuilder = new DbContextOptionsBuilder<GymDbContext>();
        optionsBuilder.UseSqlite("Data Source=GymManagementDb.sqlite");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        
        try
        {
            using (var context = new GymDbContext(optionsBuilder.Options))
            {
                Console.WriteLine("Calling EnsureCreated()...");
                var created = context.Database.EnsureCreated();
                Console.WriteLine("EnsureCreated() completed.")
                
                if (created)
                {
                    Console.WriteLine("Database created successfully!");
                }
                else
                {
                    Console.WriteLine("Database already exists.");
                }
                
                // Check if tables exist
                Console.WriteLine("Checking tables...");
                var tableCount = context.Model.GetEntityTypes()
                    .Where(t => t.BaseType == null)
                    .Count();
                Console.WriteLine("Total entity types: ");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {.Message}");
            Console.WriteLine("Stack trace: {.StackTrace}");
            throw;
        }
        
        Console.WriteLine("Done!");
    }
}
