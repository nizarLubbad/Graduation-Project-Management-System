using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        // Supervisor relation (Many Projects -> One Supervisor)
        [Required]
        public int SupervisorId { get; set; }
        [ForeignKey("SupervisorId")]
        public Supervisor Supervisor { get; set; } = null!;

        // One Project -> Many Students
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
