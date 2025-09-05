using Framework.Data.Models.Conteudo;
using System.Collections.Generic;

namespace Framework.Services.Interfaces.Core
{
    public interface IPageSectionService
    {
        public IEnumerable<PageSection> GetPageSectionsByOwner(int ownerId, string ownerType);
        PageSection GetById(int id);
        void AddPageSection(PageSection pageSection);
        void UpdatePageSection(PageSection pageSection);
        void DeletePageSection(int pageSectionId);
    }

}
