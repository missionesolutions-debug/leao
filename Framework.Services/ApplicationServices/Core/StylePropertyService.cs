using Framework.Data.Models.Conteudo;
using Framework.Repositories;
using Framework.Services.Interfaces.Core;
using System.Collections.Generic;

namespace Framework.Services.ApplicationServices.Core
{
    public class StylePropertyService : IStylePropertyService
    {
        private readonly IRepository<StyleProperty> _stylePropertyRepository;

        public StylePropertyService(IRepository<StyleProperty> stylePropertyRepository)
        {
            _stylePropertyRepository = stylePropertyRepository;
        }

        public IEnumerable<StyleProperty> GetStyleProperties(int ownerId, string ownerType)
        {
            return _stylePropertyRepository.List(sp => sp.OwnerId == ownerId && sp.OwnerType == ownerType);
        }

        public void AddStyleProperty(StyleProperty styleProperty)
        {
            _stylePropertyRepository.Add(styleProperty);
        }

        public void UpdateStyleProperty(StyleProperty styleProperty)
        {
            _stylePropertyRepository.Update(styleProperty);
        }

        public void DeleteStyleProperty(int id)
        {
            var styleProperty = _stylePropertyRepository.GetById(id);
            if (styleProperty != null)
            {
                _stylePropertyRepository.Delete(styleProperty);
            }
        }
    }

}
