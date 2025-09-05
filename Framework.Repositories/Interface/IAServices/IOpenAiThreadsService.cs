using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Services.Interfaces.IAServices
{
    public interface IOpenAiThreadsService
	{
		// Cria a thread e retorna o threadId
		Task<string> CreateThreadAsync(List<ChatMessage> initialMessages);

		// Adiciona uma mensagem à thread e retorna um resultado (pode ser vazio ou confirmação)
		Task AddMessageToThreadAsync(string threadId, ChatMessage message);

		// Cria um run para a thread, enviando instruções e ferramentas (se necessário)
		Task<RunResponse> CreateRunForThreadAsync(string threadId, string instructions, List<Tool> tools);
        Task<RunResponse> CreateRunForThreadAsync(string threadId, string instructions, List<Tool> tools, string assistantId);

        Task<RunResponse> CreateRunForThreadMapAsync(string threadId, string instructions, List<Tool> tools);

        // Recupera o status do run (para polling, se necessário)
        Task<RunResponse> RetrieveRunStatusAsync(string threadId, string runId);

		// Lista as mensagens da thread para obter o histórico atualizado
		Task<List<ChatMessage>> ListThreadMessagesAsync(string threadId);
	}

	public class Tool
	{
		public string Type { get; set; }
	}

	public class RunResponse
	{
		public string RunId { get; set; }
		public string Status { get; set; }      // e.g.: "pending", "completed", etc.
		public string MessageContent { get; set; } // Se a resposta do assistente vier aqui
	}

	public class ChatMessage
	{
		public string Role { get; set; }    // "system", "user" ou "assistant"
		public string Content { get; set; } // Texto da mensagem
	}
}
