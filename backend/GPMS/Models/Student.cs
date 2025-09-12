using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS_MALAK.Models
{
    public class Student
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public required string Email { get; set; }

        [Required, Column(TypeName = "varchar(100)")]
<<<<<<< Updated upstream
        public required string Password { get; set; }

        public ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();
=======
        public required string PasswordHash { get; set; }
        //shall we keep this line or make the Team a non nullable value
        public bool Status { get; set; }    
        public ICollection<StudentTask> StudentTask { get; set; } = new List<StudentTask>();
        public Team? Team { get; set; }
>>>>>>> Stashed changes
    }

}
