using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Reply
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("Feedback")]
        public long FeedbackId { get; set; }
        public Feedback Feedback { get; set; } = null!;

        [ForeignKey("Student")]
        public long? StudentId { get; set; }
        public Student? Student { get; set; }

        [ForeignKey("Supervisor")]
        public long? SupervisorId { get; set; }
        public Supervisor? Supervisor { get; set; }
    }
}
