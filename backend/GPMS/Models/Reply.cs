using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Reply
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        // Foreign key to Feedback
        [ForeignKey("Feedback")]
        public int FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        // Optional sender: could be a Student
        [ForeignKey("Student")]
        public long? StudentId { get; set; }
        public Student? Student { get; set; }

        // Optional sender: could be a Supervisor
        [ForeignKey("Supervisor")]
        public long? SupervisorId { get; set; }
        public Supervisor? Supervisor { get; set; }
    }
}

