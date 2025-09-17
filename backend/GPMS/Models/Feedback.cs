using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Feedback
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public required string Content { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        [ForeignKey("Team")]
        public long TeamId { get; set; }
        public Team Team { get; set; } = null!;

        [ForeignKey("Supervisor")]
        public long? SupervisorId { get; set; }
        public Supervisor? Supervisor { get; set; }
        public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    }
}
