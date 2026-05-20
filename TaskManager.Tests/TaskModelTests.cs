namespace TaskManager.Tests;

public class TaskModelTests
{
    [Fact] // This tells the computer: "This is a test!"
    public void TaskModel_Creation_SetsPropertiesCorrectly()
    {
        // Arrange
        var title = "Test My Code";
        // Act
        var task = new TaskManager.CLI.Models.ProjectTask
        {
            Title = title,
            IsCompleted = false
        };
        // Assert
        Assert.Equal(title, task.Title);
        Assert.False(task.IsCompleted);
    }
}
