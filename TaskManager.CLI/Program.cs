using Microsoft.Extensions.DependencyInjection;
using TaskManager.CLI.Data;
using TaskManager.CLI.Exceptions;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Persistence;
using TaskManager.CLI.Services;

var serviceCollection = new ServiceCollection();

serviceCollection.AddDbContext<AppDbContext>();
serviceCollection.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
serviceCollection.AddTransient<TaskService>();

var serviceProvider = serviceCollection.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

var taskService = serviceProvider.GetRequiredService<TaskService>();
bool running = true;

while (running)
{
    try
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine("    JAKARTA TASK MANAGER (VER 1.1)     ");
        Console.WriteLine("========================================");

        var tasks = await taskService.GetAllTasksAsync();
        if (!tasks.Any())
        {
            Console.WriteLine("\n [ No tasks found. Start by adding one! ]");
        }
        else
        {
            foreach (var t in tasks)
            {
                var status = t.IsCompleted ? "[DONE]" : "[PENDING]";
                Console.WriteLine($"{t.Id}. {status} {t.Title}");
            }
        }

        Console.WriteLine("\n----------------------------------------");
        Console.WriteLine("[A] Add Task | [C] Complete | [D] Delete | [Q] Quit");
        Console.Write("Choose an option: ");

        var input = Console.ReadKey(true).Key;

        switch (input)
        {
            case ConsoleKey.A:
                Console.Write("\n\nEnter Task Title: ");
                var title = Console.ReadLine() ?? "";
                await taskService.CreateTaskAsync(title);
                break;

            case ConsoleKey.C:
                Console.Write("\n\nEnter Task ID to mark as Completed: ");
                if (int.TryParse(Console.ReadLine(), out int cId))
                {
                    await taskService.MarkAsCompletedAsync(cId);
                }
                break;

            case ConsoleKey.D:
                Console.Write("\n\nEnter Task ID to Delete: ");
                if (int.TryParse(Console.ReadLine(), out int dId))
                {
                    await taskService.DeleteTaskAsync(dId);
                    Console.WriteLine("\nTask deleted successfully!");
                }
                break;

            case ConsoleKey.Q:
                running = false;
                break;
        }
    }
    catch (EntityNotFoundException ex)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n\n[NOT FOUND] {ex.Message}");
        Console.ResetColor();
        Console.WriteLine("Press any key to return to menu...");
        Console.ReadKey();
    }
    catch (ArgumentException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n\n[VALIDATION ERROR] {ex.Message}");
        Console.ResetColor();
        Console.WriteLine("Press any key to try again...");
        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n\n[CRITICAL ERROR] Something went wrong: {ex.Message}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        running = false;
    }
}

Console.WriteLine("\nTerima Kasih! Closing Task Manager...");
