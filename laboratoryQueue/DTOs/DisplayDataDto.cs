// DTOs/DisplayDataDto.cs
using laboratoryqueue.Models;
using System.Text.Json.Serialization;

namespace laboratoryqueue.DTOs
{
    public class DisplayDataDto
    {
        public QueueTicket CurrentTicket { get; set; }
        [JsonPropertyName("waitingTickets")]
        public List<QueueTicket> WaitingTickets { get; set; }
    }
}