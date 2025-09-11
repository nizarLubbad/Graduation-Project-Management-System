using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Project
    {


        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string ProjectTitle { get; set; }
        public int ProjectId { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        public string Description { get; set; }

        public bool projectStatus { get; set; } = false;
        public DateTime CreatedDate { get; set; }

        // One-to-One with Team
        public Team Team { get; set; }
    }
}
