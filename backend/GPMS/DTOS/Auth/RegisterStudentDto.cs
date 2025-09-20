namespace GPMS.DTOS.Auth
{
    public class RegisterStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long StudentId { get; set; }
        public string Department { get; set; }
    }
}
