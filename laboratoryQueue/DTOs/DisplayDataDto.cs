// DTOs/DisplayDataDto.cs
using laboratoryqueue.Models;

namespace laboratoryqueue.DTOs
{
    public class DisplayDataDto
    {
        public QueueTicket CurrentTicket { get; set; }
        public List<QueueTicket> WaitingTickets { get; set; }
    }
}