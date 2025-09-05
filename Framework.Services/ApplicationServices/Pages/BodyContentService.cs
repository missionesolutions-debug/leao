using Framework.Domain.Site.Core;
using System.Reflection;

namespace Framework.Services.ApplicationServices.Pages
{
    public class BodyContentService
    {
        public Body Build<T>(T obj)
        {
            // Usando reflexão para obter os valores das propriedades
            var bodyScripts = GetPropertyValue<string>(obj, "BodyScripts");

            return new Body()
            {
                BodyScripts = bodyScripts
            };
        }

        private TValue GetPropertyValue<TValue>(object obj, string propertyName)
        {
            // Obtém o tipo do objeto
            var type = obj.GetType();

            // Obtém a propriedade pelo nome
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (property == null || !typeof(TValue).IsAssignableFrom(property.PropertyType))
            {
                return default;
            }

            // Retorna o valor da propriedade
            return (TValue)property.GetValue(obj);
        }
    }
}
