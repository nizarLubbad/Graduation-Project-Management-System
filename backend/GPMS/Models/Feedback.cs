using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS_MALAK.Models
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
        public required KanbanTask KanbanTask { get; set; }
    }
}
