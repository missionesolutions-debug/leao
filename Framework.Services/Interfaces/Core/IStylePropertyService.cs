using Framework.Data.Models.Conteudo;
using System.Collections.Generic;

namespace Framework.Services.Interfaces.Core
{
    public interface IStylePropertyService
    {
        IEnumerable<StyleProperty> GetStyleProperties(int ownerId, string ownerType);
        void AddStyleProperty(StyleProperty styleProperty);
        void UpdateStyleProperty(StyleProperty styleProperty);
        void DeleteStyleProperty(int id);
    }

}
