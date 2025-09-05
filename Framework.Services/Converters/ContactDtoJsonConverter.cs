using Framework.Domain.Core.Forms;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Framework.Services.Converters
{
    public class ContactDtoJsonConverter : JsonConverter<ContactDTO>
    {
        public override ContactDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dto = new ContactDTO();

            // Leitura do objeto JSON
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                foreach (var property in doc.RootElement.EnumerateObject())
                {
                    switch (property.Name.ToLower())
                    {
                        case "name":
                            dto.Name = property.Value.GetString();
                            break;
                        case "email":
                            dto.Email = property.Value.GetString();
                            break;
                        case "phone":
                            dto.Phone = property.Value.GetString();
                            break;
                        case "subject":
                            dto.Subject = property.Value.GetString();
                            break;
                        case "message":
                            dto.Message = property.Value.GetString();
                            break;
                        default:
                            dto.AdditionalFields[property.Name] = property.Value.GetString(); // ou converter para o tipo apropriado
                            break;
                    }
                }
            }

            return dto;
        }

        public override void Write(Utf8JsonWriter writer, ContactDTO value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString("name", value.Name);
            writer.WriteString("email", value.Email);
            writer.WriteString("phone", value.Phone);
            writer.WriteString("subject", value.Subject);
            writer.WriteString("message", value.Message);

            foreach (var field in value.AdditionalFields)
            {
                JsonSerializer.Serialize(writer, field.Value, options);
            }

            writer.WriteEndObject();
        }
    }

}
