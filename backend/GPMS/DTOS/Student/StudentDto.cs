namespace GPMS.DTOS.Student
{
    public class StudentDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Status { get; set; }
        public string Department { get; set; } = string.Empty;
        //public long? TeamId { get; set; }
    }
}
