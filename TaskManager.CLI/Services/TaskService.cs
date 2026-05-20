using TaskManager.CLI.Exceptions;
using TaskManager.CLI.Interfaces;
using TaskManager.CLI.Models;

namespace TaskManager.CLI.Services;

public class TaskService
{
    private readonly IRepository<ProjectTask> _repository;

    // The DI Container will automatically "Inject" the repository here
    public TaskService(IRepository<ProjectTask> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProjectTask>> GetAllTasksAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task CreateTaskAsync(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.");

        var newTask = new ProjectTask
        {
            Title = title,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(newTask);
    }

    public async Task MarkAsCompletedAsync(int id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            throw new EntityNotFoundException("ProjectTask", id);

        task.IsCompleted = true;
        await _repository.UpdateAsync(task);
    }

    public async Task DeleteTaskAsync(int id)
    {
        // Business Rule: Maybe you can't delete completed tasks? 
        // You would put that logic here!
        await _repository.DeleteAsync(id);
    }
}
