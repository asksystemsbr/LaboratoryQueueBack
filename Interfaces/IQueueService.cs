﻿// Interfaces/IQueueService.cs
using laboratoryqueue.DTOs;
using laboratoryqueue.Models;
using System.Threading.Tasks;

namespace laboratoryqueue.Interfaces
{
    public interface IQueueService
    {
        Task<QueueTicket> GenerateTicketAsync(string serviceTypeCode);
        Task<QueueTicket> CallNextAsync(int counterId);
        Task<DisplayDataDto> GetDisplayDataAsync();
    }
}