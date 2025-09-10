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
        public required string Password { get; set; }

        public ICollection<StudentTask> StudentTasks { get; set; } = new List<StudentTask>();
    }

}
