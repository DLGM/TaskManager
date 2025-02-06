using System.ComponentModel.DataAnnotations;

namespace TaskManager.API.Models
{
    public class TaskItem
    {
        public int Id { get; set; } // primary key - auto increment
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty; // max chars is 100, required prop

        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt {get; set; } = DateTime.UtcNow;
    }
}