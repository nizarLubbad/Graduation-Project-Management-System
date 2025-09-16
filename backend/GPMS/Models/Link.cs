using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Link
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public required string Url { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string? Description { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
        //[ForeignKey("KanbanTask")]
        //public int TaskId { get; set; }
        //public required KanbanTask KanbanTask { get; set; }
    }
}
