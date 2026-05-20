namespace TaskManager.CLI.Exceptions;

// We inherit from 'Exception' so C# treats this as a real error
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, int id)
        : base($"{entityName} with ID {id} was not found.")
    {
    }
}
