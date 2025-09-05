using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Services.Interfaces.IAServices
{
	public interface IOpenAiRunsService
	{
		/// <summary>
		/// Cria um novo run (conversa) usando a API de Runs da OpenAI.
		/// </summary>
		/// <param name="messages">Mensagens iniciais para contextualizar o run.</param>
		/// <returns>O ID do run criado.</returns>
		Task<string> CreateRunAsync(List<ChatMessage> messages);

		/// <summary>
		/// Adiciona uma nova mensagem ao run existente e retorna a resposta do assistente.
		/// </summary>
		/// <param name="runId">O ID do run.</param>
		/// <param name="message">A mensagem do usuário.</param>
		/// <returns>Resposta do assistente.</returns>
		Task<string> AddMessageToRunAsync(string runId, ChatMessage message);
	}
}
