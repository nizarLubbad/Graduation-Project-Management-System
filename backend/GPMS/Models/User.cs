using System.ComponentModel.DataAnnotations;

namespace GPMS.Models
{
    public class User
    {
        [Key]
        public long UserId { get; set; }
            
        //[Required, MaxLength(100)]
        //public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty; // "Student" or "Supervisor"

        public string Name { get; set; }
        public Student? Student { get; set; }
        public Supervisor? Supervisor { get; set; }
    }
}
