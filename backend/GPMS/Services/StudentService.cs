
using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.DTOS.Student;
using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GPMS.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public StudentService(IStudentRepository studentRepository, IUserRepository userRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<StudentProfileDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAll()
                .Include(s => s.User)
                .Include(s => s.Team)
                .ToListAsync();  

            return students.Select(student => new StudentProfileDto
            {
                UserId = student.UserId,
                Name = student.User?.Name ?? "N/A",
                Email = student.User?.Email ?? "N/A",
                Status = student.Status,
                Department = student.Department,
                TeamId = student.TeamId,
                TeamName = student.Team?.TeamName
            }).ToList();
        }


        public async Task<StudentProfileDto?> GetStudentProfileAsync(long userId)
        {
            var student = await _studentRepository.GetAll()
                .Include(s => s.User)
                .Include(s => s.Team)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return null;

            return new StudentProfileDto
            {
                UserId = student.UserId,
                Name = student.User.Name,
                Email = student.User.Email,
                Status = student.Status,
                Department = student.Department,
                TeamId = student.TeamId,
                TeamName = student.Team?.TeamName
            };
        }
        public async Task UpdateStudentsStatusAsync(IEnumerable<long> studentIds)
        {
            var students = await _studentRepository.GetAll()
                .Where(s => studentIds.Contains(s.UserId))
                .ToListAsync();

            foreach (var student in students)
            {
                student.Status = true;
            }

            await _studentRepository.SaveChangesAsync();
        }


        //public async Task<bool> UpdateStudentProfileAsync(long userId, UpdateStudentProfileDto dto)
        //{
        //    var student = await _studentRepository.GetByIdAsync(userId);
        //    if (student == null) return false;

        //    var user = await _userRepository.GetByIdAsync(userId);
        //    if (user == null) return false;

        //    user.Name = dto.Name;
        //    student.Department = dto.Department;

        //    await _userRepository.UpdateAsync(user);
        //    await _studentRepository.UpdateAsync(student);

        //    return true;
        //}


    }
}