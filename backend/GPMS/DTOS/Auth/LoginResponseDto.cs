namespace GPMS.DTOS.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Name{ get; set; }
        
        public long UserId { get; set; }
        
        public string? Email { get; set; }
        public bool? Status { get; set; } = false;



    }
}