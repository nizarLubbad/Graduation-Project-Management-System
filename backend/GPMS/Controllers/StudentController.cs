
using GPMS.DTOS.Student;
using GPMS.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GPMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Student")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();

                _logger.LogInformation("Retrieved {Count} students successfully.", students.Count);

                return Ok(new
                {
                    success = true,
                    count = students.Count,
                    students
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving students.");
                return BadRequest(new
                {
                    success = false,
                    message = "Error retrieving students: " + ex.Message
                });
            }
        }
        [HttpPatch("update-status")]
        public async Task<IActionResult> UpdateStudentsStatus([FromBody] List<long> studentIds)
        {
            try
            {
                await _studentService.UpdateStudentsStatusAsync(studentIds);
                return Ok(new { message = "Students' status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating students' status.");
                return StatusCode(500, "Internal server error");
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetStudentById(long id)
        //{
        //    try
        //    {
        //        var student = await _studentService.GetStudentByIdAsync(id);
        //        if (student == null)
        //        {
        //            _logger.LogWarning("Student with ID {Id} not found.", id);
        //            return NotFound(new { success = false, message = "Student not found" });
        //        }

        //        _logger.LogInformation("Retrieved student {Id} successfully.", id);
        //        return Ok(new { success = true, student });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while retrieving student with ID {Id}.", id);
        //        return BadRequest(new { success = false, message = "Error retrieving student: " + ex.Message });
        //    }
        //}
        //[HttpPut("profile")]
        //public async Task<IActionResult> UpdateProfile([FromBody] UpdateStudentProfileDto dto)
        //{
        //    var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        //    var result = await _studentService.UpdateStudentProfileAsync(userId, dto);

        //    return result ? Ok(new { message = "Profile updated successfully" }) : BadRequest();
        //}
    }
}