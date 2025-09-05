using System.Threading.Tasks;

namespace Framework.Services.Interfaces.IAServices
{
	public interface IOpenAiService
	{
		/// <summary>
		/// Cria uma nova conversa exclusiva com o agente.
		/// </summary>
		/// <returns>O ID da conversa criada.</returns>
		Task<string> CreateConversationAsync();

		/// <summary>
		/// Envia uma mensagem para a conversa existente e obtém a resposta do agente.
		/// </summary>
		/// <param name="conversationId">O ID da conversa.</param>
		/// <param name="message">A mensagem do usuário.</param>
		/// <returns>A resposta do agente.</returns>
		Task<string> SendMessageAsync(string conversationId, string message);
	}
}
