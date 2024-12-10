
namespace Hustle.Models
{

    public enum TaskPriority
    {
        Low ,
        Medium ,
        High
    }
    public class TodoItems
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; } 
        public bool IsPinned { get; set; }

    }
}
