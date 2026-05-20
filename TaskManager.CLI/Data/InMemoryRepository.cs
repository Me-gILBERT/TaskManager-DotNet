using TaskManager.CLI.Interfaces;

namespace TaskManager.CLI.Data;

public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    public List<T> Items { get; } = [];

    public async Task<IEnumerable<T>> GetAllAsync() => await Task.FromResult(Items);
    public async Task<T?> GetByIdAsync(int id)
    => await Task.FromResult(Items.FirstOrDefault(x => x.Id == id));

    public async Task AddAsync(T entity)
    {
        entity.Id = Items.Any() ? Items.Max(x => x.Id) + 1 : 1;
        Items.Add(entity);
        await Task.CompletedTask;
    }
    public async Task UpdateAsync(T entity)
    {
        var existing = Items.FirstOrDefault(x => x.Id == entity.Id);
        if (existing != null)
        {
            var index = Items.IndexOf(existing);
            Items[index] = entity;
        }
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item != null) Items.Remove(item);
        await Task.CompletedTask;
    }
}