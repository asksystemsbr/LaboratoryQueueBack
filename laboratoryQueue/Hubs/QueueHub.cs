// Hubs/QueueHub.cs
using Microsoft.AspNetCore.SignalR;
using laboratoryqueue.Models;

namespace laboratoryqueue.Hubs
{
    public class QueueHub : Hub
    {
        public async Task UpdateQueue(QueueTicket ticket)
        {
            await Clients.All.SendAsync("ReceiveQueueUpdate", ticket);
        }
    }
}