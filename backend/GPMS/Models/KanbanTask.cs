using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static GPMS.Models.Enums;

namespace GPMS.Models
{
    public class KanbanTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public PriorityLevel Priority { get; set; } // Enum: Low, Medium, High or 1, 2, 3


        [Column(TypeName = "varchar(100)")]
        public string status { get; set; } = "To Do"; // Possible values: "To Do", "In Progress", "Done"

        public bool? IsCompleted { get; set; } = false;
        [ForeignKey("Team")]
        public long TeamId { get; set; } // Foreign key to  Team
        public Team Team { get; set; }
        public ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();
        public ICollection<Link> Links { get; set; } = new List<Link>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    }
}
