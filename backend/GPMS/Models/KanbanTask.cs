using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskStatusEnum = GPMS.Models.Enums.TaskStatus;
using TaskPriorityEnum = GPMS.Models.Enums.TaskPriority;

namespace GPMS.Models
{
    public class KanbanTask
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.ToDo;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Team")]
        public long TeamId { get; set; }
        public Team Team { get; set; } = null!;
        public ICollection<Student> AssignedStudents { get; set; } = new List<Student>();

    }
}
