using System.ComponentModel.DataAnnotations;

namespace GPMS.DTOS.Student
{
    public class UpdateStudentProfileDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [MaxLength(100, ErrorMessage = "Department cannot exceed 100 characters")]
        public string? Department { get; set; }
    }
}
