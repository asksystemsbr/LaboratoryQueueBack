using laboratoryqueue.Models;
using PasswordPrinter.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Timers;

namespace PasswordPrinter.Services
{
    public class PollingService
    {
        private readonly System.Timers.Timer _timer;
        private readonly HttpClient _httpClient;
        private readonly Action<string> _logAction;

        public PollingService(Action<string> logAction)
        {
            _logAction = logAction;
            _timer = new System.Timers.Timer(3000); // Intervalo de 3 segundos
            _timer.Elapsed += async (sender, e) => await PollAsync();
            _timer.AutoReset = true;
            _httpClient = new HttpClient();
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        private async Task PollAsync()
        {
            try
            {
                _logAction("Verificando tickets pendentes...");
                var response = await _httpClient.GetAsync("http://localhost:7035/api/queue/pending-tickets");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tickets = JsonSerializer.Deserialize<List<QueueTicket>>(content);

                    foreach (var ticket in tickets)
                    {
                        _logAction($"Imprimindo ticket: {ticket.Number}");
                        PrinterHelper.ImprimirSenha(ticket.Number, ticket.ServiceType.Name, ticket.IssuedAt);
                        await ConfirmPrintAsync(ticket.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logAction($"Erro no polling: {ex.Message}");
            }
        }

        private async Task ConfirmPrintAsync(int ticketId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://localhost:7035/api/queue/confirm-print", ticketId);
                if (!response.IsSuccessStatusCode)
                {
                    _logAction($"Erro ao confirmar impressão do ticket {ticketId}");
                }
            }
            catch (Exception ex)
            {
                _logAction($"Erro ao confirmar impressão: {ex.Message}");
            }
        }
    }
}
