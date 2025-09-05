using Framework.Data.Models.Conteudo;
using Framework.Repositories.Interface.Core;
using Framework.Services.Interfaces.Core;
using System.Collections.Generic;

namespace Framework.Services.ApplicationServices.Core
{
    public class PageSectionItemService : IPageSectionItemService
    {
        private readonly IPageSectionItemRepository _pageSectionItemRepository;

        public PageSectionItemService(IPageSectionItemRepository pageSectionItemRepository)
        {
            _pageSectionItemRepository = pageSectionItemRepository;
        }
        public  IEnumerable<PageSectionItem> GetPageSectionItems(int pageSectionId)
        {
            return _pageSectionItemRepository.GetPageSectionItemsByPageSectionId(pageSectionId);
        }

        public  PageSectionItem GetPageSectionItemById(int id)
        {
            return _pageSectionItemRepository.GetById(id);
        }

        public void AddPageSectionItem(PageSectionItem pageSectionItem)
        {
            _pageSectionItemRepository.Add(pageSectionItem);
        }

        public void UpdatePageSectionItem(PageSectionItem pageSectionItem)
        {
            _pageSectionItemRepository.Update(pageSectionItem);
        }

        public void DeletePageSectionItem(int pageSectionItemId)
        {
            var pageSectionItem = _pageSectionItemRepository.GetById(pageSectionItemId);
            if (pageSectionItem != null)
            {
                pageSectionItem.Excluido = true;
                pageSectionItem.Ativo = false;
                _pageSectionItemRepository.Update(pageSectionItem);
            }
        }
    }
}
