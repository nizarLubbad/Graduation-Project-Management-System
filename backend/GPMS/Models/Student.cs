using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Student
    {

        public long StudentId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string Name { get; set; }

        public bool Status { get; set; } = false;
        public string Department { get; set; }

        [Required, Column(TypeName = "varchar(100)")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required, Column(TypeName = "varchar(100)")]
        public required string PasswordHash { get; set; }
        [Required]
        //-----------------------------
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        //-------------------------------
        public ICollection<StudentTask> StudentTask { get; set; } = new List<StudentTask>();
        [ForeignKey("Team")]
        public long? TeamId { get; set; }
        public Team Team { get; set; }
        public ICollection<Reply> Replys { get; set; } = new List<Reply>();
        
    }

}
