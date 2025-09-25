namespace GPMS.DTOS.Auth
{
    public class UserDto
    {
        
        
            public long UserId { get; set; }
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string? Department { get; set; }
            public bool? Status { get; set; } 
            public int? TeamCount { get; set; } 
            public int? MaxTeams { get; set; } 
        
    }
}
