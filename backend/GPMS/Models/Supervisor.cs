
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Supervisor
    {
        [Key, ForeignKey("User")]
        public long UserId { get; set; }

        [MaxLength(100)]
        public string? Department { get; set; }

        public int TeamCount { get; set; }
        public int MaxTeams { get; set; } = 5;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Reply> Replys { get; set; } = new List<Reply>();
    }
}
