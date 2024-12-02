// Models/QueueTicket.cs
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace laboratoryqueue.Models
{
    public class QueueTicket
    {
        public int Id { get; set; }
        [JsonPropertyName("numero")]
        public string Number { get; set; }
        public int ServiceTypeId { get; set; }
        public int? CounterId { get; set; }
        public string Status { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime? CalledAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }

        public virtual QueueServiceType ServiceType { get; set; }
        public virtual QueueCounter Counter { get; set; }
    }
}