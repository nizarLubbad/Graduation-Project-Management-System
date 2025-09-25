using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class User
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public long UserId { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required, MaxLength(50)]
        public string Role { get; set; } = null!; // "Student" or "Supervisor"

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        // Navigation properties
        public Student? Student { get; set; }
        public Supervisor? Supervisor { get; set; }
    }
}
