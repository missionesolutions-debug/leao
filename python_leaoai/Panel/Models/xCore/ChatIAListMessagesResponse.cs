using System.Collections.Generic;

namespace Panel.Models.xCore
{
    public class ChatIAListMessagesResponse
    {
        public List<MessageChatResponse> messageChatResponses { get; set; } = new List<MessageChatResponse>();
    }

    public class MessageChatResponse()
    {
        public string Message { get; set; } = "";
        public bool IsChatAnswer { get; set; }
        public string Time { get; set; } = "";
    }
}
