
using Hustle.Models;

namespace Hustle.Services
{
    public interface ITodoServices
    {
        Task SaveTodoAsync(TodoItems todoItem);

        Task UpdateTodoAsync(TodoItems todoItem);

        Task DeleteTodoAsync(TodoItems todoItem);

        Task MarkAsDoneAsync(Guid todoId);

        Task<List<TodoItems>> LoadTodoAsync();
    }
}
