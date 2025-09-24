//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace GPMS.Models
//{
//    public class Student
//    {
//        // المفتاح الأساسي هو UserId (مش StudentId مستقل)
//        [Key, ForeignKey(nameof(User))]
//        public long UserId { get; set; }

//        public bool Status { get; set; } = false;
//        public string? Department { get; set; }

//        // العلاقة مع User
//        public User User { get; set; } = null!;

//        [ForeignKey("Team")]
//        public long? TeamId { get; set; }
//        public Team? Team { get; set; }

//        public ICollection<Reply> Replies { get; set; } = new List<Reply>();
//        public ICollection<Link> Links { get; set; } = new List<Link>();
//    }
//}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Student
    {
        [Key, ForeignKey("User")]
        public long UserId { get; set; }

        public bool? Status { get; set; } = false;

        [MaxLength(100)]
        public string? Department { get; set; }

        [ForeignKey("Team")]
        public long? TeamId { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public Team? Team { get; set; }
        public ICollection<Reply> Replies { get; set; } = new List<Reply>();
        public ICollection<Link> Links { get; set; } = new List<Link>();
    }
}
