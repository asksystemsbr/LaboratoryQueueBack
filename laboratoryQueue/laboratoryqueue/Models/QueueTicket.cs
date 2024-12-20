using System.Text.Json.Serialization;

namespace laboratoryqueue.Models
{
    public class QueueTicket
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("numero")]
        public string Number { get; set; }

        [JsonPropertyName("serviceTypeId")]
        public int ServiceTypeId { get; set; }

        public int? CounterId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("issuedAt")]
        public DateTime IssuedAt { get; set; }

        [JsonPropertyName("horario")]
        public DateTime? CalledAt { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("tipo")]
        public virtual QueueServiceType ServiceType { get; set; }

        public virtual QueueCounter Counter { get; set; }

        [JsonPropertyName("printStatus")]
        public string PrintStatus { get; set; } // Valores possíveis: "PENDING", "PRINTED"

        [JsonPropertyName("userId")]
        public string? UserId { get; set; } // Novo campo opcional
    }
}
