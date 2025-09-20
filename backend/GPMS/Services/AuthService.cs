using GPMS.DTOS.Auth;
using GPMS.Helpers;
using GPMS.Interfaces;
using GPMS.Models;
using System.Security.Claims;

namespace GPMS.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ISupervisorRepository _supervisorRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(
            IUserRepository userRepo,
            IStudentRepository studentRepo,
            ISupervisorRepository supervisorRepo,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider)
        {
            _userRepo = userRepo;
            _studentRepo = studentRepo;
            _supervisorRepo = supervisorRepo;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task RegisterStudentAsync(RegisterStudentDto dto)
        {
            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                throw new Exception("Email already exists");

            var hashedPassword = _passwordHasher.HashPassword(dto.Password);

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                StudentId = dto.StudentId,
                Department = dto.Department
            };

            await _studentRepo.AddAsync(student);
            await _studentRepo.SaveChangesAsync();
        }

        public async Task RegisterSupervisorAsync(RegisterSupervisorDto dto)
        {
            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                throw new Exception("Email already exists");

            var hashedPassword = _passwordHasher.HashPassword(dto.Password);

            var supervisor = new Supervisor
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            await _supervisorRepo.AddAsync(supervisor);
            await _supervisorRepo.SaveChangesAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Invalid email or password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = _jwtProvider.GenerateToken(claims);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role.ToString()
            };
        }
    }
}
