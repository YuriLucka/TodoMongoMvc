using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TodoMongoMvc.Models;
using TodoMongoMvc.Settings;

namespace TodoMongoMvc.Services;

public class TodoService
{
    private readonly IMongoCollection<TodoItem> _todos;

    public TodoService(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _todos = database.GetCollection<TodoItem>(settings.TodoCollectionName);
    }

    public async Task<List<TodoItem>> GetAllAsync() =>
        await _todos.Find(_ => true).SortByDescending(t => t.CreatedAt).ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(string id) =>
        await _todos.Find(t => t.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TodoItem todo) =>
        await _todos.InsertOneAsync(todo);

    public async Task UpdateAsync(string id, TodoItem todo) =>
        await _todos.ReplaceOneAsync(t => t.Id == id, todo);

    public async Task DeleteAsync(string id) =>
        await _todos.DeleteOneAsync(t => t.Id == id);
}
