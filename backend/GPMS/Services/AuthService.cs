using GPMS.DTOS.Auth;
using GPMS.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GPMS.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Register Student
        public async Task<LoginResponseDto> RegisterStudentAsync(RegisterStudentDto dto)
        {
           
            if (_context.Students.Any(s => s.Email == dto.Email))
                throw new Exception("Email already registered");

            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = GenerateToken(student.StudentId, student.Email, "Student"),
                UserId = student.StudentId,
                Role = "Student"
            };
        }

        // Register Supervisor 
        public async Task<LoginResponseDto> RegisterSupervisorAsync(RegisterSupervisorDto dto)
        {
            
            if (_context.Supervisors.Any(s => s.Email == dto.Email))
                throw new Exception("Email already registered");

            var supervisor = new Supervisor
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Supervisors.Add(supervisor);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                Token = GenerateToken(supervisor.SupervisorId, supervisor.Email, "Supervisor"),
                UserId = supervisor.SupervisorId,
                Role = "Supervisor"
            };
        }

        //  Login
        public async Task<LoginResponseDto?> LoginAsync(LoginDto dto, string role)
        {
            if (role == "Student")
            {
                var student = _context.Students.FirstOrDefault(s => s.Email == dto.Email);
                if (student == null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
                    return null;

                return new LoginResponseDto
                {
                    Token = GenerateToken(student.StudentId, student.Email, "Student"),
                    UserId = student.StudentId,
                    Role = "Student"
                };
            }
            else if (role == "Supervisor")
            {
                var supervisor = _context.Supervisors.FirstOrDefault(s => s.Email == dto.Email);
                if (supervisor == null || !BCrypt.Net.BCrypt.Verify(dto.Password, supervisor.PasswordHash))
                    return null;

                return new LoginResponseDto
                {
                    Token = GenerateToken(supervisor.SupervisorId, supervisor.Email, "Supervisor"),
                    UserId = supervisor.SupervisorId,
                    Role = "Supervisor"
                };
            }

            return null;
        }

        // Generate JWT Token
        private string GenerateToken(long id, string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
