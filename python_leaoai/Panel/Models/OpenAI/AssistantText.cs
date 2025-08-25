using System.Collections.Generic;

namespace Api.Models.xCode.Chat.OpenAI
{
    public class AssistantText
    {
        public string value { get; set; }
        public List<object> annotations { get; set; }
    }

    public class AssistantMessageItem
    {
        public string type { get; set; }
        public AssistantText text { get; set; }
    }
}
