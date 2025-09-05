using Data.Models.Core;
using Framework.Domain.Core.Forms;
using System.Collections.Generic;

namespace Framework.Services.Mapper.Forms
{
    public class ContactMapper
    {
        public static Contact Map(ContactDTO dto)
        {
            var contact = new Contact
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Subject = dto.Subject,
                Message = dto.Message,
                // Mapear outros campos fixos conforme necessário
            };

            // Mapear os campos adicionais
            foreach (var field in dto.AdditionalFields)
            {
                var propInfo = typeof(Contact).GetProperty(field.Key);

                if (propInfo != null && propInfo.CanWrite)
                {
                    var convertedValue = Convert.ChangeType(field.Value, Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType);
                    propInfo.SetValue(contact, convertedValue);
                }
                else
                {
                    // Se o campo não existir em Contact, você pode decidir ignorá-lo
                    // ou lidar com ele de outra forma, como adicionar a um log.
                }
            }

            return contact;
        }

        public static ContactDTO Map(Contact contact)
        {
            var dto = new ContactDTO
            {
                Name = contact.Name,
                Email = contact.Email,
                Phone = contact.Phone,
                Subject = contact.Subject,
                Message = contact.Message,
                // Mapear outros campos fixos conforme necessário
            };

            // Mapear os campos adicionais de volta ao DTO
            foreach (var prop in typeof(Contact).GetProperties())
            {
                if (prop.CanRead && prop.GetValue(contact) != null && !IsFixedProperty(prop.Name))
                {
                    dto.AdditionalFields[prop.Name] = prop.GetValue(contact);
                }
            }

            return dto;
        }

        private static bool IsFixedProperty(string propertyName)
        {
            // Lista de propriedades fixas que não devem ser mapeadas para AdditionalFields
            var fixedProperties = new HashSet<string>
        {
            nameof(Contact.Id),
            nameof(Contact.Ativo),
            nameof(Contact.Excluido),
            nameof(Contact.DataCriacao),
            nameof(Contact.DataEdicao),
            nameof(Contact.TipoContactId),
            nameof(Contact.ProdutoId),
            nameof(Contact.Name),
            nameof(Contact.Email),
            nameof(Contact.Phone),
            nameof(Contact.Subject),
            nameof(Contact.Message),
            nameof(Contact.CurriculumFile),
            nameof(Contact.CurriculumUrl),
            nameof(Contact.CurriculumFileName)
        };

            return fixedProperties.Contains(propertyName);
        }
    }

}
