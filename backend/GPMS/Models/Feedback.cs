using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Content { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;


        [ForeignKey("KanbanTask")]
        public int TaskId { get; set; }
        public KanbanTask KanbanTask { get; set; }


        [ForeignKey("Supervisor")]
        public long? SupervisorId { get; set; } // Foreign key to Supervisor
        public Supervisor? Supervisor { get; set; }
        public ICollection<Reply> Replys { get; set; } = new List<Reply>();


    }
}
