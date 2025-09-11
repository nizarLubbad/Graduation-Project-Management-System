using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Team
    {
        public long TeamId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string TeamName { get; set; }

        public bool TeamStatus { get; set; } = false;

        public DateTime CreatedDate { get; set; }

        // One-to-One with Project
        [ForeignKey("Project")]
        public string ProjectTitle { get; set; }
        public Project Project { get; set; }

        // Many-to-One with Supervisor
        [ForeignKey("Supervisor")]
        public long SupervisorId { get; set; }
        public Supervisor Supervisor { get; set; }

        // One-to-One with ToDo, Doing, Done
        public Todo Todo { get; set; }
        public Doing Doing { get; set; }
        public Done Done { get; set; }

        [ForeignKey("Student")]
        public long StudentId { get; set; }
        public Student Student { get; set; }
        //public ICollection<Student> Students { get; set; }
    }
}
