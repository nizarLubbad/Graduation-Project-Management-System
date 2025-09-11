using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Student
    {
        
        public long StudentId { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        [Required, Column(TypeName = "varchar(100)")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required, Column(TypeName = "varchar(100)")]
        public required string PasswordHash { get; set; }

        public ICollection<StudentTask> StudentTask { get; set; } = new List<StudentTask>();
        public Team Team { get; set; }
    }

}
