using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using laboratoryqueue.Interfaces;
using laboratoryqueue.Models;
using laboratoryqueue.Hubs;
using laboratoryqueue.DTOs;

namespace laboratoryqueue.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private readonly IHubContext<QueueHub> _hubContext;

        public QueueController(
            IQueueService queueService,
            IHubContext<QueueHub> hubContext)
        {
            _queueService = queueService;
            _hubContext = hubContext;
        }

        [HttpOptions]
        [Route("generate")]
        public IActionResult Options()
        {
            Response.Headers.Append("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
            return Ok();
        }


        [HttpPost("generate")]
        public async Task<ActionResult<QueueTicket>> GenerateTicket([FromBody] GenerateTicketDto request)
        {
            var ticket = await _queueService.GenerateTicketAsync(request.ServiceTypeCode);
            await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate", ticket);
            return Ok(ticket);
        }

        [HttpGet("display")]
        public async Task<ActionResult<DisplayDataDto>> GetDisplayData()
        {
            var data = await _queueService.GetDisplayDataAsync();
            return Ok(data);
        }

        [HttpPost("call")]
        public async Task<ActionResult<QueueTicket>> CallNext([FromBody] CallNextDto request)
        {
            var ticket = await _queueService.CallNextAsync(request.CounterId);
            if (ticket != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveQueueUpdate", ticket);
            }
            return Ok(ticket);
        }
    }
}