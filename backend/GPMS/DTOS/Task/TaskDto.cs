namespace GPMS.DTOS.Task
{
    public class TaskDto
    {
        public int Id { get; set; } // for Todo/Doing/Done entities (they had int Id)
        public string Title { get; set; }
        public string Description { get; set; }
        public long TeamId { get; set; }
        public string Status { get; set; } // "Todo"/"Doing"/"Done"
        public DateTime CreatedDate { get; set; }
    }
}
