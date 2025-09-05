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
    public class OpenAiThreadsService : IOpenAiThreadsService
	{
		private readonly HttpClient _httpClient;
		private readonly string _assistantId;
        private readonly string _assistantMapId;
        private readonly string _apiKey;
		private readonly string _vectorStoreId;

		public OpenAiThreadsService(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_assistantId = "asst_dxaJjs3lBHUSiV6qhqRdYHjq"; //configuration["OpenAi:AssistantId"];
			_apiKey = "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";//configuration["OpenAi:ApiKey"];
            //_vectorStoreId = configuration["OpenAi:VectorStoreId"];
		}

		// Cria a thread com mensagens iniciais (por exemplo, uma mensagem "system")
		public async Task<string> CreateThreadAsync(List<ChatMessage> initialMessages)
		{
			var endpoint = "https://api.openai.com/v1/threads";
			var requestBody = new
			{
			};

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			// Adiciona o header beta
			request.Headers.Add("OpenAI-Beta", "assistants=v2");
			request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var json = JObject.Parse(await response.Content.ReadAsStringAsync());

			// Supondo que o thread ID esteja no campo "id"
			return json["id"]?.ToString();
		}


		// Adiciona uma mensagem à thread
		public async Task AddMessageToThreadAsync(string threadId, ChatMessage message)
		{
			var endpoint = $"https://api.openai.com/v1/threads/{threadId}/messages";
			var requestBody = new
			{
				role = message.Role,
				content = message.Content
			};

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			// Adiciona o header beta
			request.Headers.Add("OpenAI-Beta", "assistants=v2");
			request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			// Aqui não precisamos necessariamente retornar conteúdo.
		}

        // Cria um run para a thread (para processar as novas mensagens)
        public async Task<RunResponse> CreateRunForThreadMapAsync(string threadId, string instructions, List<Tool> tools)
        {
            var endpoint = $"https://api.openai.com/v1/threads/{threadId}/runs";
            var requestBody = new
            {
                assistant_id = _assistantMapId,
                //instructions = instructions,
                tools = tools.Select(t => new { type = t.Type }).ToArray()
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            // Adiciona o header beta
            request.Headers.Add("OpenAI-Beta", "assistants=v2");
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new RunResponse
            {
                RunId = json["id"]?.ToString(),
                Status = json["status"]?.ToString(),
                MessageContent = json["message"]?["content"]?.ToString()
            };
        }

        public async Task<RunResponse> CreateRunForThreadAsync(string threadId, string instructions, List<Tool> tools)
        {
            var endpoint = $"https://api.openai.com/v1/threads/{threadId}/runs";
            var requestBody = new
            {
                assistant_id = _assistantId,
                //instructions = instructions,
                tools = tools.Select(t => new { type = t.Type }).ToArray()
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            // Adiciona o header beta
            request.Headers.Add("OpenAI-Beta", "assistants=v2");
            request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());

            return new RunResponse
            {
                RunId = json["id"]?.ToString(),
                Status = json["status"]?.ToString(),
                MessageContent = json["message"]?["content"]?.ToString()
            };
        }

        public async Task<RunResponse> CreateRunForThreadAsync(string threadId, string instructions, List<Tool> tools, string assistantId)
		{
			var endpoint = $"https://api.openai.com/v1/threads/{threadId}/runs";
			var requestBody = new
			{
				assistant_id = assistantId,
				//instructions = instructions,
				tools = tools.Select(t => new { type = t.Type }).ToArray()
			};

			using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			// Adiciona o header beta
			request.Headers.Add("OpenAI-Beta", "assistants=v2");
			request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var json = JObject.Parse(await response.Content.ReadAsStringAsync());

			return new RunResponse
			{
				RunId = json["id"]?.ToString(),
				Status = json["status"]?.ToString(),
				MessageContent = json["message"]?["content"]?.ToString()
			};
		}

		// Recupera o status do run (para polling, se necessário)
		public async Task<RunResponse> RetrieveRunStatusAsync(string threadId, string runId)
		{
			var endpoint = $"https://api.openai.com/v1/threads/{threadId}/runs/{runId}";
			using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			// Adiciona o header beta
			request.Headers.Add("OpenAI-Beta", "assistants=v2");
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var json = JObject.Parse(await response.Content.ReadAsStringAsync());

			return new RunResponse
			{
				RunId = json["id"]?.ToString(),
				Status = json["status"]?.ToString(),
				MessageContent = json["message"]?["content"]?.ToString()
			};
		}

		// Lista as mensagens da thread (para recuperar o histórico ou a resposta final)
		public async Task<List<ChatMessage>> ListThreadMessagesAsync(string threadId)
		{
			var endpoint = $"https://api.openai.com/v1/threads/{threadId}/messages";
			using var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
			// Adiciona o header beta
			request.Headers.Add("OpenAI-Beta", "assistants=v2");
			var response = await _httpClient.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var json = JObject.Parse(await response.Content.ReadAsStringAsync());

			// Supondo que as mensagens estejam em um array "data"
			var messages = new List<ChatMessage>();
			foreach (var msg in json["data"])
			{
				messages.Add(new ChatMessage
				{
					Role = msg["role"]?.ToString(),
					Content = msg["content"]?.ToString()
				});
			}
			return messages;
		}
	}
}
