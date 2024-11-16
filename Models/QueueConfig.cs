// Models/QueueConfig.cs
namespace laboratoryqueue.Models
{
    public class QueueConfig
    {
        public int Id { get; set; }
        public int QtdSenhasPainel { get; set; }
        public int IntervaloAtualizacao { get; set; }
        public string MensagemRodape { get; set; }
        public bool SomAtivo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; }
    }
}