// Interfaces/IPrinterService.cs
using laboratoryqueue.Models;

namespace laboratoryqueue.Interfaces
{
    public interface IPrinterService
    {
        void PrintTicket(QueueTicket ticket);
    }
}