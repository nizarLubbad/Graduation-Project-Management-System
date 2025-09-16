using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GPMS.Models
{
    public class Link
    {
        [Key]
        public long Id { get; set; }   

        [Required]
        [Column(TypeName = "varchar(500)")]
        public required string Url { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public required string Title { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        
        [ForeignKey("Student")]
        public long StudentId { get; set; }
        public Student Student { get; set; } = null!;

      
        [ForeignKey("Team")]
        public long TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}
