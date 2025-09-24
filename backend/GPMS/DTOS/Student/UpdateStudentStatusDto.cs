namespace GPMS.DTOS.Student
{
    public class UpdateStudentStatusDto
    {
        public int StudentId { get; set; }
        public bool? Status { get; set; } = false;
    }
}
