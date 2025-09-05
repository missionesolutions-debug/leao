using System;

namespace Framework.Data.Models.xCode
{
    public class ChatIAItem
    {
        public int Id { get; set; }
        public int ChatIAId { get; set; }
        public int UsuarioId { get; set; }
        public string Mensagem { get; set; }
        public string Tipo { get; set; } // "user" ou "openai"
        public DateTime DataEnvio { get; set; }

    }
}
