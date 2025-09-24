using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TeamId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string TeamName { get; set; }



        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public Project? Project { get; set; }

        // Many-to-One with Supervisor
        [ForeignKey("Supervisor")]
        public long? SupervisorId { get; set; }
        public Supervisor? Supervisor { get; set; }

        // One-to-Many with KanbanTask
        public ICollection<KanbanTask> KanbanTasks { get; set; } = new List<KanbanTask>();

        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();


        //public long StudentId { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Link> Links { get; set; } = new List<Link>();










        /////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////
        //public ICollection<Student> Students { get; set; }
        //public Todo Todo { get; set; }
        //public Doing Doing { get; set; }
        //public Done Done { get; set; }
    }
}
