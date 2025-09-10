using System.ComponentModel.DataAnnotations;

namespace GPMS_MALAK.Models
{
    public class KanbanTask
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();
        public ICollection<Link> Links { get; set; } = new List<Link>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    }
}
