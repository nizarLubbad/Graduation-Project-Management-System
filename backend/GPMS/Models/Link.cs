using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public required string Url { get; set; }

        [ForeignKey("KanbanTask")]
        public int TaskId { get; set; }
        public required KanbanTask KanbanTask { get; set; }
    }
}
