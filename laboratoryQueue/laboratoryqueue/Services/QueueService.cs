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

        public async Task<QueueTicket> GenerateTicketAsync(string serviceTypeCode)
        {
            var serviceType = await _context.QueueServiceTypes
                .FirstOrDefaultAsync(st => st.Code == serviceTypeCode);

            if (serviceType == null)
                throw new ArgumentException("Tipo de serviço inválido");

            var lastTicket = await _context.QueueTickets
                .Where(t => t.ServiceType.Code == serviceTypeCode)
                .OrderByDescending(t => t.Number)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastTicket != null)
            {
                nextNumber = int.Parse(lastTicket.Number.Substring(1)) + 1;
            }

            var ticket = new QueueTicket
            {
                Number = $"{serviceTypeCode}{nextNumber:D3}",
                ServiceTypeId = serviceType.Id,
                Status = "WAITING",
                IssuedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Active = true
            };

            _context.QueueTickets.Add(ticket);
            await _context.SaveChangesAsync();

            _printerService.PrintTicket(ticket);

            return ticket;
        }

        public async Task<QueueTicket> CallNextAsync(int counterId)
        {
            var nextTicket = await _context.QueueTickets
                .Include(t => t.ServiceType)
                .Where(t => t.Status == "WAITING" && t.Active)
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
                .Where(t => t.Status == "CALLED")
                .OrderByDescending(t => t.CalledAt)
                .FirstOrDefaultAsync();

            var waitingTickets = await _context.QueueTickets
                .Include(t => t.ServiceType)
                .Where(t => t.Status == "WAITING" && t.Active)
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
    }
}