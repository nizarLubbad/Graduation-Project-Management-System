using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Supervisor
    {
        public long SupervisorId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR(100)")]
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Column(TypeName = "VARCHAR(15)")]
        [Required]
        public string Department { get; set; }
        //public string SubmitStudent { get; set; }

        // One-to-Many with Teams
        public ICollection<Team> Teams { get; set; } = new List<Team>();

    }
}
