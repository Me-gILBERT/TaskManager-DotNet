using TaskManager.CLI.Interfaces;

namespace TaskManager.CLI.Models;

public class ProjectTask : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Priority { get; set; } = "Medium";

    public override string ToString() => $"[{Id}] {(IsCompleted ? "[X]" : "[ ]")} {Title}";
}