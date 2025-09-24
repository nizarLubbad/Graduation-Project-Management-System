
using GPMS.DTOS.Auth;
using GPMS.Helpers;
using GPMS.Interfaces;
using GPMS.Models;
using GPMS.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
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
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository userRepo,
            IStudentRepository studentRepo,
            ISupervisorRepository supervisorRepo,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _studentRepo = studentRepo;
            _supervisorRepo = supervisorRepo;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _logger = logger;
        }

        public async Task RegisterStudentAsync(RegisterStudentDto dto)
        {
            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                UserId = dto.UserId,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Role = "Student",
                Name = dto.Name
            };

            var student = new Student
            {
                UserId = user.UserId,
                Status = false,
                Department = dto.Department
            };

            await _userRepo.AddAsync(user);
            await _studentRepo.AddAsync(student);

            await _userRepo.SaveChangesAsync();
        }

        public async Task RegisterSupervisorAsync(RegisterSupervisorDto dto)
        {
            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                UserId = dto.UserId,
                Email = dto.Email,
                PasswordHash = _passwordHasher.HashPassword(dto.Password),
                Role = "Supervisor",
                Name = dto.Name
            };

            var supervisor = new Supervisor
            {
                UserId = user.UserId,
                Department = dto.Department,
                TeamCount = 0,
                MaxTeams = 5
            };

            await _userRepo.AddAsync(user);
            await _supervisorRepo.AddAsync(supervisor);

            await _userRepo.SaveChangesAsync();
        }

        //public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        //{
        //    var user = await _userRepo.GetByEmailAsync(dto.Email);
        //    if (user == null)
        //        throw new UnauthorizedAccessException("Invalid email");

        //    if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
        //        throw new UnauthorizedAccessException("Invalid password");

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //        new Claim(ClaimTypes.Role, user.Role),
        //        new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
        //        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) 
        //    };

        //    var token = _jwtProvider.GenerateToken(claims);
        //    bool hasTeam = false;

        //    if (user.Role == "Student")
        //    {
        //        var student = await _studentRepo.GetByIdAsync(user.UserId);

        //        if (student != null)
        //        {
        //            hasTeam = student.Status ?? false ;
        //        }
        //    }

        //    return new LoginResponseDto
        //    {
        //        Token = token,
        //        UserId = user.UserId,
        //        Name = user.Name,
        //        Email = user.Email,
        //        Role = user.Role,
        //        Status = hasTeam
        //    };

        //}
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid email");

            if (!_passwordHasher.VerifyPassword(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid password");

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
    };

            var token = _jwtProvider.GenerateToken(claims);

            bool statusFromDb = false;

            if (user.Role == "Student")
            {
                var student = await _studentRepo.GetByIdAsync(user.UserId);

                if (student != null)
                {
                    statusFromDb = student.Status ?? false;
                }
            }

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Status = statusFromDb
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();

            return users.Select(user => new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,

                Department = user.Student != null ? user.Student.Department :
                             user.Supervisor != null ? user.Supervisor.Department : null,

                Status = user.Student != null ? user.Student.Status : null,

                TeamCount = user.Supervisor != null ? user.Supervisor.TeamCount : null,
                MaxTeams = user.Supervisor != null ? user.Supervisor.MaxTeams : null
            });
        }
        public async Task<bool?> GetUserStatusAsync(long userId)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return null;
                }

                if (user.Role == "Student")
                {
                    var student = await _studentRepo.GetByIdAsync(user.UserId);
                    if (student == null)
                    {
                        _logger.LogWarning("Student details for user ID {UserId} not found.", userId);
                        return null;
                    }
                    return student.Status;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user status for ID {UserId}.", userId);
                return null;
            }
        }
        public async Task<UserDto?> GetUserByIdAsync(long userId)
        {
            try
            {
                var user = await _userRepo.GetByIdWithDetailsAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", userId);
                    return null;
                }

                return new UserDto
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Department = user.Student?.Department ?? user.Supervisor?.Department,
                    Status = user.Student?.Status,
                    TeamCount = user.Supervisor?.TeamCount,
                    MaxTeams = user.Supervisor?.MaxTeams
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user with ID {UserId}.", userId);
                return null;
            }
        }
        public async Task<int> DeleteAllUsersAsync()
        {
            try
            {
                _logger.LogInformation("Starting process to delete all users and related data");

                var deletedCount = await _userRepo.DeleteAllUsersAsync();

                _logger.LogInformation("Successfully deleted {DeletedCount} records", deletedCount);

                return deletedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting all users");
                throw new Exception("Failed to delete all users: " + ex.Message, ex);
            }
        }

    }
}
