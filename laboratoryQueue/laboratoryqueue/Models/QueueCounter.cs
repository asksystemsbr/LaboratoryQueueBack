// Models/QueueCounter.cs
namespace laboratoryqueue.Models
{
    public class QueueCounter
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}