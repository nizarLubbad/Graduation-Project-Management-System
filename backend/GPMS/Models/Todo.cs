using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Todo
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        // One-to-One with Team
        [ForeignKey("Team")]
        public long TeamId { get; set; }
        public Team Team { get; set; }
    }
}
