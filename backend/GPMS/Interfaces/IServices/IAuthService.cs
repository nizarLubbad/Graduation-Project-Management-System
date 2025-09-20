using GPMS.DTOS.Auth;

namespace GPMS.Interfaces
{
    public interface IAuthService
    {
        Task RegisterStudentAsync(RegisterStudentDto dto);

        Task RegisterSupervisorAsync(RegisterSupervisorDto dto);

        Task<LoginResponseDto> LoginAsync(LoginDto dto);
    }
}
