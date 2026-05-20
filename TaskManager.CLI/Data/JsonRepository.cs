using System.Text.Json;
using TaskManager.CLI.Exceptions;
using TaskManager.CLI.Interfaces;

namespace TaskManager.CLI.Data;

public class JsonRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly string _filePath;

    public JsonRepository(string fileName)
    {
        // AppDomain.CurrentDomain.BaseDirectory finds your project's 'bin' folder
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
    }

    // --- Private Helpers (The "Plumbing") ---

    private async Task<List<T>> LoadFromFileAsync()
    {
        if (!File.Exists(_filePath)) return [];

        var json = await File.ReadAllTextAsync(_filePath);
        // Deserialization: JSON Text -> C# List
        return JsonSerializer.Deserialize<List<T>>(json) ?? [];
    }

    private async Task SaveToFileAsync(List<T> items)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(items, options);
        // Serialization: C# List -> JSON Text
        await File.WriteAllTextAsync(_filePath, json);
    }

    // --- Interface Implementation (The "Contract") ---

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await LoadFromFileAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var items = await LoadFromFileAsync();
        return items.FirstOrDefault(x => x.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        var items = await LoadFromFileAsync();
        entity.Id = items.Any() ? items.Max(x => x.Id) + 1 : 1;
        items.Add(entity);
        await SaveToFileAsync(items);
    }

    public async Task UpdateAsync(T entity)
    {
        var items = await LoadFromFileAsync();
        var existing = items.FirstOrDefault(x => x.Id == entity.Id);

        // GUARD CLAUSE
        if (existing == null)
        {
            throw new EntityNotFoundException(typeof(T).Name, entity.Id);
        }

        var index = items.IndexOf(existing);
        items[index] = entity;
        await SaveToFileAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadFromFileAsync();
        var item = items.FirstOrDefault(x => x.Id == id);

        // GUARD CLAUSE: If the user types an ID that doesn't exist
        if (item == null)
        {
            throw new EntityNotFoundException(typeof(T).Name, id);
        }

        items.Remove(item);
        await SaveToFileAsync(items);
    }


}