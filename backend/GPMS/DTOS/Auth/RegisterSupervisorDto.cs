using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.DTOS.Auth
{
    public class RegisterSupervisorDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;
        [Required] 
        public long UserId { get; set; }
        [MaxLength(100)]
        public string? Department { get; set; }
    }
}