using AutoMapper;
using GPMS.DTOS.Student;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, CreateStudentDto>();
        }
    }
}
