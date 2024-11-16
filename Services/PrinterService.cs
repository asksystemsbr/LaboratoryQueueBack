// Services/PrinterService.cs
using laboratoryqueue.Interfaces;
using laboratoryqueue.Models;

namespace laboratoryqueue.Services
{
    public class PrinterService : IPrinterService
    {
        public void PrintTicket(QueueTicket ticket)
        {
            // Implementação da impressão do ticket
            // Pode usar System.Drawing.Printing para impressão real
            Console.WriteLine($"Imprimindo ticket: {ticket.Number}");
        }
    }
}