using Org.BouncyCastle.Bcpg.OpenPgp;

namespace GPMS.DTOS.Auth
{
    public class RegisterStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string  Role { get; set; }

    }
}