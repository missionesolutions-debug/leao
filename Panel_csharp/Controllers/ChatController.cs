using Api.Models.xCode.Chat.OpenAI;
using Data;
using Data.Models.Core;
using Data.Models.Pessoa;
using Framework.Data.Models.xCode;
using Framework.Repositories.Factories.xCode;
using Framework.Services.Interfaces.IAServices;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Panel.Models.xCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatIARepository _chatIARepository;
        private readonly IOpenAiThreadsService _openAiThreadsService;
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context, ChatIARepository chatIARepository, IOpenAiThreadsService openAiThreadsService)
        {
            _context = context;
            _chatIARepository = chatIARepository;
            _openAiThreadsService = openAiThreadsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userx = User.Identity.Name;

            Usuario usuario = _context.Usuario.Where(b => b.Login == userx).FirstOrDefault();


            int id = 0;


            if (usuario != null)
            {
                if (_context.ChatIAs.Where(b => b.UsuarioId == usuario.Id && b.IsPrompt != true).Any())
                    id = _context.ChatIAs.Where(b => b.UsuarioId == usuario.Id && b.IsPrompt != true).FirstOrDefault().Id;
                else
                {
                    var initialMessages = new List<ChatMessage>
                    {
                        new ChatMessage { Role = "openai", Content = "Você é um assistente útil e prestativo." }
                    };

                    // Cria a thread na OpenAI
                    string threadId = await _openAiThreadsService.CreateThreadAsync(initialMessages);

                    if (!string.IsNullOrEmpty(threadId))
                    {
                        var chatNew = new ChatIA
                        {
                            UsuarioId = usuario.Id,
                            DataCriacao = DateTime.Now,
                            ThreadId = threadId,
                            IsPrompt = false,
                            Guid = Helpers.GenerateGuid()
                        };


                        var createdChatIA = await _chatIARepository.CreateChatIAAsync(chatNew);

                        id = createdChatIA.Id;


                        if (createdChatIA != null && createdChatIA.Id > 0)
                        {

                            var chatIAItem = new ChatIAItem()
                            {
                                ChatIAId = createdChatIA.Id,
                                Tipo = "system",
                                DataEnvio = DateTime.Now,
                                Mensagem = "🦁💬\r\nOlá! Como posso ajudar você hoje com questões relacionadas ao processo legal de marcas no Brasil? Se você tiver uma dúvida específica, por favor, descreva-a para que eu possa fornecer a melhor orientação possível."
                            };

                            await _chatIARepository.AddChatIAItemAsync(chatIAItem);
                        }
                    }
                }
            }
            else
            {

            }


            ChatIA chatIA = await _chatIARepository.GetChatIAByIdAsync(id);

            var result = new ChatIAListMessagesResponse();

            foreach (var item in chatIA.ChatIAItems.Where(b => b.Tipo != "openaimap").OrderBy(m => m.DataEnvio))
            {
                result.messageChatResponses.Add(new MessageChatResponse
                {
                    Message = item.Mensagem,
                    IsChatAnswer = item.UsuarioId > 0 ? false : true,
                    Time = new DateTimeOffset(item.DataEnvio).ToUnixTimeSeconds().ToString()
                });
            }

            // Passa o chatIAId para a view (para que seja enviado em cada post)
            ViewBag.ChatIAId = id;


            return View(result);
        }

        // Rota para receber a mensagem e retornar uma resposta mock
        [HttpPost]
        public async Task<IActionResult> SendMessage(string message, int chatId)
        {
            int id = chatId;

            // Simula um pequeno atraso para imitar o processamento do servidor
            await Task.Delay(500);

            // 1. Busca o chat
            var chat = await _chatIARepository.GetChatIAByIdAsync(id);
            if (chat == null)
                throw new Exception("Chat não encontrado.");

            // 2. Salva a mensagem do usuário no banco de dados
            var userItem = new ChatIAItem
            {
                ChatIAId = id,
                UsuarioId =15,
                Mensagem = message,
                Tipo = "user",
                DataEnvio = DateTime.Now
            };

            await _chatIARepository.AddChatIAItemAsync(userItem);

            if (string.IsNullOrEmpty(chat.ThreadId))
                throw new Exception("Thread (run) não definida.");

            //await ExecuteOpenAICallAsync(id, dto, chat, userItem);


            // 3. Adiciona a mensagem do usuário à thread na OpenAI
            var userMessage = new ChatMessage
            {
                Role = "user",
                Content = message
            };
            await _openAiThreadsService.AddMessageToThreadAsync(chat.ThreadId, userMessage);

            // 4. Chama o run para processar a nova mensagem, enviando instruções e ferramentas, se necessário.
            // Você pode ajustar as instruções conforme sua lógica.
            //var instructions = "Siga as instruções do assistente treinado. Todas as viagens serão terrestre, nunca sugerir áereo ou por mar.";
            var instructions = "";
            var tools = new List<Tool>
            {
                //new Tool { Type = "code_interpreter" },
                new Tool { Type = "file_search" }
            };
            var runResponse = await _openAiThreadsService.CreateRunForThreadAsync(chat.ThreadId, instructions, tools, "asst_vIt1fR78R1OvDw3zUaztNbHY");

            // 5. (Opcional) Polling: aguarda até que o run esteja concluído.
            while (runResponse.Status != "completed")
            {
                await Task.Delay(1000);
                runResponse = await _openAiThreadsService.RetrieveRunStatusAsync(chat.ThreadId, runResponse.RunId);
            }

            // 6. Recupera o histórico atualizado da thread para obter a resposta final do assistente.
            var messages = await _openAiThreadsService.ListThreadMessagesAsync(chat.ThreadId);


            var assistantMessageJson = messages.FirstOrDefault(m => m.Role == "assistant")?.Content;
            string assistantTextValue = string.Empty;

            if (!string.IsNullOrEmpty(assistantMessageJson))
            {
                // Desserializa o JSON para uma lista de objetos AssistantMessageItem
                var messageItems = JsonConvert.DeserializeObject<List<AssistantMessageItem>>(assistantMessageJson);
                assistantTextValue = messageItems?.FirstOrDefault()?.text?.value;
            }

            // 7. Salva a resposta do assistente no banco
            var openAiItem = new ChatIAItem
            {
                ChatIAId = id,
                UsuarioId = 0,
                Mensagem = assistantTextValue,
                Tipo = "openai",
                DataEnvio = DateTime.Now
            };
            await _chatIARepository.AddChatIAItemAsync(openAiItem);

            // Cria uma resposta mock com base na mensagem recebida
            var reply = assistantTextValue;

            return Json(new { reply });
        }


  

    }
}
