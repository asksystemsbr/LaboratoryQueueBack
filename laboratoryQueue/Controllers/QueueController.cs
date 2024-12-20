using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using laboratoryqueue.Interfaces;
using laboratoryqueue.Models;
using laboratoryqueue.Hubs;
using laboratoryqueue.DTOs;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                // Captura o ID do usuário autenticado, se disponível
                var userId = User?.FindFirst("id")?.Value;

                // Chama o serviço para gerar a senha
                var ticket = await _queueService.GenerateTicketAsync(request.ServiceTypeCode, userId);

                return Ok(new
                {
                    TicketNumber = ticket.Number,
                    Message = "Senha gerada com sucesso!"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro inesperado.", Details = ex.Message });
            }
        }

        [HttpGet("display")]
        public async Task<ActionResult<DisplayDataDto>> GetDisplayData()
        {
            var data = await _queueService.GetDisplayDataAsync();
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Console.WriteLine(json);
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

        [HttpPost("confirm-print")]
        public async Task<ActionResult> ConfirmPrint([FromBody] int ticketId)
        {
            try
            {
                await _queueService.ConfirmPrintAsync(ticketId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("pending-tickets")]
        public async Task<ActionResult<List<QueueTicket>>> GetPendingTickets()
        {
            var tickets = await _queueService.GetPendingTicketsAsync();
            return Ok(tickets);
        }


    }
}