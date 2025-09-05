using Framework.Data.Models.Conteudo;
using System.Collections.Generic;

namespace Framework.Services.Interfaces.Core
{
    public interface IPageSectionItemService
    {
        IEnumerable<PageSectionItem> GetPageSectionItems(int pageSectionId);
        PageSectionItem GetPageSectionItemById(int id);
        void AddPageSectionItem(PageSectionItem pageSectionItem);
        void UpdatePageSectionItem(PageSectionItem pageSectionItem);
        void DeletePageSectionItem(int pageSectionItemId);
    }
}
