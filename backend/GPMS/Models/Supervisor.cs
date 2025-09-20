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
        [Required, Column(TypeName = "varchar(100)")]
        public string PasswordHash { get; set; }

        [Column(TypeName = "VARCHAR(15)")]
        [Required]
        public string Department { get; set; }//in our form there is no such a field for the supervisor
        //public string SubmitStudent { get; set; }
        public int TeamCount { get; set; }
        // One-to-Many with Teams

        [Required]
        public long UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Reply> Replys { get; set; } = new List<Reply>();
        

    }
}
