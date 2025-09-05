using Framework.Services.Interfaces.IAServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repositories.Services.Implementations.IAServices
{
    public class OpenAiService : IOpenAiService
	{
		private readonly HttpClient _httpClient;
		private readonly string _assistantId;
		private readonly string _apiKey;
		private readonly string _vectorStoreId;

		public OpenAiService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			// Leia os valores do arquivo de configuração (appsettings.json)
			_assistantId = configuration["OpenAi:AssistantId"];
			_apiKey = configuration["OpenAi:ApiKey"];
			_vectorStoreId = configuration["OpenAi:VectorStoreId"];
		}

		public async Task<string> CreateConversationAsync()
		{
			// Exemplo de endpoint: ajuste conforme sua documentação!
			var endpoint = $"https://api.openai.com/v1/assistants/{_assistantId}/conversations";

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

			// Envie parâmetros necessários. Exemplo: passando o vectorStoreId
			var content = new { vectorStoreId = _vectorStoreId };
			request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var json = JObject.Parse(responseContent);
			var conversationId = json["conversationId"]?.ToString();
			return conversationId;
		}

		public async Task<string> SendMessageAsync(string conversationId, string message)
		{
			// Exemplo de endpoint para enviar mensagens: ajuste conforme necessário
			var endpoint = $"https://api.openai.com/v1/assistants/{_assistantId}/conversations/{conversationId}/messages";

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

			var body = new { message = message, vectorStoreId = _vectorStoreId };
			request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			var json = JObject.Parse(responseContent);
			var reply = json["reply"]?.ToString();
			return reply;
		}
	}
}
