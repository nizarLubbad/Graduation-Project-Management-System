using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Doing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Title { get; set; }

        public string Description { get; set; }

        // One-to-One with Team
        [ForeignKey("Team")]
        public long TeamId { get; set; }
        public Team Team { get; set; }
    }
}
