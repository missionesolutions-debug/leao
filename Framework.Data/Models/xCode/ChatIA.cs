using System;
using System.Collections.Generic;

namespace Framework.Data.Models.xCode
{
    public class ChatIA
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataCriacao { get; set; }
        public string ConversationId { get; set; }
		// Armazena o ID da conversa (thread) da OpenAI
		public string ThreadId { get; set; }
		public string Guid { get; set; }
        public string Typed { get; set; } = "chat";
        public bool IsPrompt { get; set; } = false;
        public string ProcessoContestado { get; set; } = "";
        public string MarcaContestada { get; set; } = "";
        public string ClasseContestada { get; set; } = "";
        public bool Excluido { get; set; } = false;

        // Relacionamento 1:N com ChatItem
        public List<ChatIAItem> ChatIAItems { get; set; } = new List<ChatIAItem>();
    }
}
