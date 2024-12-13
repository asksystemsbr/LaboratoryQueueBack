using System.Text.Json.Serialization;

namespace laboratoryqueue.Models
{
    public class QueueServiceType
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("priority")]
        public bool Priority { get; set; }
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
        //public ICollection<QueueTicket> QueueTickets { get; set; }
    }
}