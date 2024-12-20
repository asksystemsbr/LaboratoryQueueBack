using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using laboratoryqueue.Interfaces;
using laboratoryqueue.Models;
using laboratoryqueue.Data;
using laboratoryqueue.DTOs;

namespace laboratoryqueue.Services
{
    public class QueueService : IQueueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPrinterService _printerService;

        public QueueService(
            ApplicationDbContext context,
            IPrinterService printerService)
        {
            _context = context;
            _printerService = printerService;
        }

        public async Task<QueueTicket> GenerateTicketAsync(string serviceTypeCode, string? userId = null)
        {
            // Buscar o tipo de serviço
            var serviceType = await _context.QueueServiceTypes
                .FirstOrDefaultAsync(st => st.Code == serviceTypeCode);

            if (serviceType == null)
                throw new ArgumentException("Tipo de serviço inválido");

            // Se o UserId for fornecido, verificar se já há uma senha para o mesmo dia
            if (!string.IsNullOrEmpty(userId))
            {
                var existingTicket = await _context.QueueTickets
                    .Where(t => t.UserId == userId && t.IssuedAt.Date == DateTime.Today)
                    .FirstOrDefaultAsync();

                if (existingTicket != null)
                    throw new InvalidOperationException("Você já gerou uma senha hoje.");
            }

            // Buscar o último ticket gerado para o tipo de serviço
            var lastTicket = await _context.QueueTickets
                .Where(t => t.ServiceType.Code == serviceTypeCode)
                .OrderByDescending(t => t.Number)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastTicket != null)
            {
                try
                {
                    // Verifica se o tipo de senha contém 2 letras e ajusta o índice da substring
                    int startIndex = lastTicket.Number.Length > 1 && char.IsLetter(lastTicket.Number[0]) && char.IsLetter(lastTicket.Number[1]) ? 2 : 1;

                    // Extrai o número com base no índice calculado e incrementa
                    nextNumber = int.Parse(lastTicket.Number.Substring(startIndex)) + 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao calcular o próximo número: {ex.Message}");
                }
            }

            // Criar novo ticket
            var ticket = new QueueTicket
            {
                Number = $"{serviceTypeCode}{nextNumber:D3}",
                ServiceTypeId = serviceType.Id,
                Status = "AGUARDANDO",
                IssuedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Active = true,
                UserId = userId // Associar o UserId se fornecido
            };

            // Adicionar o ticket ao banco de dados
            _context.QueueTickets.Add(ticket);
            await _context.SaveChangesAsync();

            // Imprimir o ticket (comportamento existente)
            _printerService.PrintTicket(ticket);

            return ticket;
        }


        public async Task<QueueTicket> CallNextAsync(int counterId)
        {
            var nextTicket = await _context.QueueTickets
                .Include(t => t.ServiceType)
                .Where(t => t.Status == "AGUARDANDO" && t.Active)
                .OrderByDescending(t => t.ServiceType.Priority)
                .ThenBy(t => t.IssuedAt)
                .FirstOrDefaultAsync();

            if (nextTicket == null)
                return null;

            nextTicket.Status = "CALLED";
            nextTicket.CounterId = counterId;
            nextTicket.CalledAt = DateTime.Now;
            nextTicket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return nextTicket;
        }

        public async Task<DisplayDataDto> GetDisplayDataAsync()
        {
            var currentTicket = await _context.QueueTickets
                .Include(t => t.ServiceType)
                .Include(t => t.Counter)
                .Where(t => t.Status == "CHAMANDO")
                .OrderByDescending(t => t.CalledAt)
                .FirstOrDefaultAsync();

            var waitingTickets = await _context.QueueTickets
                .Include(t => t.ServiceType)
                .Where(t => t.Status == "AGUARDANDO" && t.Active)
                .OrderByDescending(t => t.ServiceType.Priority)
                .ThenBy(t => t.IssuedAt)
                .Take(5)
                .ToListAsync();

            return new DisplayDataDto
            {
                CurrentTicket = currentTicket,
                WaitingTickets = waitingTickets
            };
        }

        public async Task ConfirmPrintAsync(int ticketId)
        {
            var ticket = await _context.QueueTickets.FindAsync(ticketId);
            if (ticket == null) throw new ArgumentException("Ticket não encontrado.");

            ticket.PrintStatus = "PRINTED";
            _context.QueueTickets.Update(ticket);
            await _context.SaveChangesAsync();
        }


        public async Task<List<QueueTicket>> GetPendingTicketsAsync()
        {
            return await _context.QueueTickets
                  .Include(t => t.ServiceType) // Inclui os dados relacionados de ServiceType
                  .Where(t => t.PrintStatus == "PENDING" && t.Active)
                  .ToListAsync();
        }
    }
}