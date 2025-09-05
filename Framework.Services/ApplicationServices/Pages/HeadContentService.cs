using Framework.Domain.Site.Core;
using System.Reflection;

namespace Framework.Services.ApplicationServices.Pages
{
    public class HeadContentService
    {
        public Head Build<T>(T obj)
        {
            // Usando reflexão para obter os valores das propriedades
            var pageTitle = GetPropertyValue<string>(obj, "PageTitle");
            var metaDescription = GetPropertyValue<string>(obj, "MetaDescription");
            var metaImage = GetPropertyValue<string>(obj, "MetaImage");
            var imageOpenGraph = GetPropertyValue<string>(obj, "MetaImage");

            return new Head()
            {
                PageTitle = pageTitle,
                ImageOpenGraph = imageOpenGraph,
                MetaDescription = metaDescription
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
