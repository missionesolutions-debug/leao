using System.Collections.Generic;

namespace Framework.Domain.Core.Forms
{
    public class ContactDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; } = string.Empty;

        // Campos dinâmicos
        public Dictionary<string, object> AdditionalFields { get; set; } = new Dictionary<string, object>();
    }
}
