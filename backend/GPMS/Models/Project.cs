using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Project
    {
        
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string ProjectTitle { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        // Supervisor relation (Many Projects -> One Supervisor)
        [Required]
        public long SupervisorId { get; set; }
        [ForeignKey("SupervisorId")]
        public Supervisor Supervisor { get; set; } = null!;

        // One Project -> Many Students
        public ICollection<Student> Students { get; set; } = new List<Student>();
        //one to one with team
        
        [ForeignKey("Team")]
        public long? TeamId { get; set; }
        public Team? Team { get; set; }
    }
}
