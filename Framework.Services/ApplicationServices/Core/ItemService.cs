using Framework.Domain.Site.Component;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Services.ApplicationServices.Core
{
    public class ItemService
    {
        public static readonly List<string> CamposIgnorados = new List<string>
        {
            "DataEdicao",
            "Ativo",
            "Excluido",
            "Password",
            "Senha",
            "FileType",
            "FileSize",
            "PlaceReceived",
            "TableId",
            "TableAction",
            
        };

        public Item Build<T>(T obj, List<string> IgnoreFields = null)
        {
            Item item = new Item();

            if (IgnoreFields != null && IgnoreFields.Any())
                CamposIgnorados.AddRange(IgnoreFields);

            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                // Ignora campos específicos
                if (CamposIgnorados.Contains(prop.Name)) continue;

                var value = prop.GetValue(obj);
                if (value == null) continue; // Ignora campos nulos

                PropertyInfo itemProp = typeof(Item).GetProperty(prop.Name);
                if (itemProp != null && itemProp.CanWrite)
                    itemProp.SetValue(item, value);
                else
                {
                    if (prop.Name != null)
                    {
                        string fieldName = prop.Name.UppercaseFirstLetter();

                        if (CamposIgnorados.Contains(fieldName)) continue;

                        item.fields[fieldName] = value;
                    }
                }

            }

            return item;
        }

    }

}
