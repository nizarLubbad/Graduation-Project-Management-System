using GPMS.DTOS.Auth;

namespace GPMS.Interfaces
{
    public interface IAuthService
    {
        Task RegisterStudentAsync(RegisterStudentDto dto);

        Task RegisterSupervisorAsync(RegisterSupervisorDto dto);

        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool?> GetUserStatusAsync(long userId);
        Task<UserDto?> GetUserByIdAsync(long userId);
        Task<int> DeleteAllUsersAsync();


    }
}
