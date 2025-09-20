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
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Role = "Student",
                Name = dto.Name
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            //var hashedPassword = _passwordHasher.HashPassword(dto.Password);

            //var student = new Student
            //{
            //    Name = dto.Name,
            //    Email = dto.Email,
            //    PasswordHash = hashedPassword,
            //    StudentId = dto.StudentId,
            //    Department = dto.Department
            //};

            //await _studentRepo.AddAsync(student);
            //await _studentRepo.SaveChangesAsync();
            var student = new Student
            {
                StudentId = dto.StudentId,
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = user.PasswordHash, 
                Department = dto.Department,
                Status = false,
                UserId = user.UserId,
                TeamId = null 
            };
            await _studentRepo.AddAsync(student);
            await _studentRepo.SaveChangesAsync();
        }

        public async Task RegisterSupervisorAsync(RegisterSupervisorDto dto)
        {
            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                throw new Exception("Email already exists");
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Role = "Supervisor",
                Name = dto.Name
            };

            //var hashedPassword = _passwordHasher.HashPassword(dto.Password);

            //var supervisor = new Supervisor
            //{
            //    Name = dto.Name,
            //    Email = dto.Email,
            //    PasswordHash = hashedPassword
            //};
            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();
            var supervisor = new Supervisor
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = user.PasswordHash,
                //TeamCount = 0,
                UserId = user.UserId
            };


            await _supervisorRepo.AddAsync(supervisor);
            await _supervisorRepo.SaveChangesAsync();
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email ");

            if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid Password ");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Name)


            };

            var token = _jwtProvider.GenerateToken(claims);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role,
                Name = user.Name

            };
        }
    }
}
