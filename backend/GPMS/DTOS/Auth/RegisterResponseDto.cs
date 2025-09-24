namespace GPMS.DTOS.Auth
{
    public class RegisterResponseDto
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
