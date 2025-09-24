namespace GPMS.DTOS.Task
{
    public class TaskDto
    {
        //public int Id { get; set; } // for Todo/Doing/Done entities (they had int Id)
        //public string Title { get; set; }
        //public string Description { get; set; }
        //public long TeamId { get; set; }
        //public string Status { get; set; } // "Todo"/"Doing"/"Done"
        //public DateTime CreatedDate { get; set; }
        public long Id { get; set; } // Task primary key
        public string? Title { get; set; }
        public string? Description { get; set; }
        public long TeamId { get; set; }
        public string Status { get; set; } = null!; // "Todo"/"Doing"/"Done"
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; } = "Medium";
        public IEnumerable<string> AssignedStudentNames { get; set; } = new List<string>(); // Names of assigned students
        public DateTime CreatedDate { get; set; }
    }
}
