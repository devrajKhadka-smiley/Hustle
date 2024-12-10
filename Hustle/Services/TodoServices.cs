using Hustle.Models;
using System.Text.Json;

namespace Hustle.Services
{
    public class TodoServices : ITodoServices
    {
        private readonly string filePath = Path.Combine(AppContext.BaseDirectory, "TodosDatabase.json");

        // Load all Todo items
        public async Task<List<TodoItems>> LoadTodoAsync()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<TodoItems>();
                }

                var json = await File.ReadAllTextAsync(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<TodoItems>();
                }

                return JsonSerializer.Deserialize<List<TodoItems>>(json) ?? new List<TodoItems>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                return new List<TodoItems>();
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while loading TodoItems: {ioEx.Message}");
                return new List<TodoItems>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while loading TodoItems: {ex.Message}");
                return new List<TodoItems>();
            }
        }

        // Save a new Todo item
        public async Task SaveTodoAsync(TodoItems todoItem)
        {
            try
            {
                var todos = await LoadTodoAsync();
                todos.Add(todoItem); // Add the new item

                await SaveTodosToFileAsync(todos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving TodoItem: {ex.Message}");
            }
        }

        // Update an existing Todo item
        public async Task UpdateTodoAsync(TodoItems todoItem)
        {
            try
            {
                var todos = await LoadTodoAsync();
                var existingTodo = todos.FirstOrDefault(t => t.Id == todoItem.Id);

                if (existingTodo != null)
                {
                    existingTodo.Title = todoItem.Title;
                    existingTodo.Description = todoItem.Description;
                    existingTodo.DueDate = todoItem.DueDate;
                    existingTodo.IsCompleted = todoItem.IsCompleted;

                    await SaveTodosToFileAsync(todos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating TodoItem: {ex.Message}");
            }
        }

        // Delete a Todo item
        public async Task DeleteTodoAsync(TodoItems todoItem)
        {
            try
            {
                var todos = await LoadTodoAsync();
                var todoToDelete = todos.FirstOrDefault(t => t.Id == todoItem.Id);

                if (todoToDelete != null)
                {
                    todos.Remove(todoToDelete);
                    await SaveTodosToFileAsync(todos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while deleting TodoItem: {ex.Message}");
            }
        }

        // Mark a Todo item as completed
        public async Task MarkAsDoneAsync(Guid todoId)
        {
            try
            {
                var todos = await LoadTodoAsync();
                var todo = todos.FirstOrDefault(t => t.Id == todoId);

                if (todo != null)
                {
                    todo.IsCompleted = true;
                    await SaveTodosToFileAsync(todos);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while marking TodoItem as done: {ex.Message}");
            }
        }

        // Helper method to save all Todos to the file
        private async Task SaveTodosToFileAsync(List<TodoItems> todos)
        {
            try
            {
                var json = JsonSerializer.Serialize(todos, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"I/O error while saving TodoItems: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while saving TodoItems: {ex.Message}");
            }
        }
    }
}
