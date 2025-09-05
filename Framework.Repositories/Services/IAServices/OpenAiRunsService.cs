using Framework.Services.Interfaces.IAServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repositories.Services.Implementations.IAServices
{
    public class OpenAiRunsService : IOpenAiRunsService
	{
		private readonly HttpClient _httpClient;
		private readonly string _assistantId;
		private readonly string _apiKey;
		private readonly string _vectorStoreId;

		public OpenAiRunsService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_assistantId = configuration["OpenAi:AssistantId"];
			_apiKey = configuration["OpenAi:ApiKey"];
			_vectorStoreId = configuration["OpenAi:VectorStoreId"];
		}

		public async Task<string> CreateRunAsync(List<ChatMessage> messages)
		{
			var endpoint = "https://api.openai.com/v1/runs";
			var requestBody = new
			{
				assistant_id = _assistantId,
				model = "gpt-4", // ou o modelo que você treinou
				messages = messages.Select(m => new { role = m.Role, content = m.Content }).ToArray(),
				vector_store_id = _vectorStoreId  // se for aplicável; caso não, remova essa propriedade
			};

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			var json = JObject.Parse(content);

			// Supondo que o run_id seja retornado no campo "id"
			return json["id"]?.ToString();
		}

		public async Task<string> AddMessageToRunAsync(string runId, ChatMessage message)
		{
			var endpoint = $"https://api.openai.com/v1/runs/{runId}/messages";
			var requestBody = new
			{
				message = new { role = message.Role, content = message.Content }
			};

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			var json = JObject.Parse(content);

			// Supondo que a resposta do assistente esteja em "message.content"
			return json["message"]?["content"]?.ToString();
		}
	}
}