using Microsoft.EntityFrameworkCore;
using TaskManager.CLI.Models;

namespace TaskManager.CLI.Persistence;

public class AppDbContext : DbContext
{
    // This tells EF Core to create a table called "Tasks" based on our ProjectTask model
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // We are telling EF Core to use a SQLite file named "taskmanager.db"
        optionsBuilder.UseSqlite("Data Source=taskmanager.db");
    }
}