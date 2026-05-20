using Microsoft.AspNetCore.Mvc;
using TaskManager.CLI.Exceptions;
using TaskManager.CLI.Models;
using TaskManager.CLI.Services;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;

    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        try
        {
            await _taskService.CreateTaskAsync(request.Title);
            return Ok(new { message = "Task created successfully." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> MarkComplete(int id)
    {
        try
        {
            await _taskService.MarkAsCompletedAsync(id);
            return Ok(new { message = "Task marked as completed." });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _taskService.DeleteTaskAsync(id);
            return Ok(new { message = "Task deleted successfully." });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}

public class CreateTaskRequest
{
    public string Title { get; set; } = string.Empty;
}
