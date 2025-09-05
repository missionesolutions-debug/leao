using Framework.Data.Models.Conteudo;
using Framework.Repositories.Interface.Core;
using Framework.Services.Interfaces.Core;
using System.Collections.Generic;

namespace Framework.Services.ApplicationServices.Core
{
    public class PageSectionService : IPageSectionService
    {
        private readonly IPageSectionRepository _pageSectionRepository;

        public PageSectionService(IPageSectionRepository pageSectionRepository)
        {
            _pageSectionRepository = pageSectionRepository;
        }

        // Métodos para PageSection

        public IEnumerable<PageSection> GetPageSectionsByOwner(int ownerId, string ownerType)
        {
            return _pageSectionRepository.GetPageSectionsByOwner(ownerId, ownerType);
        }

        public PageSection GetById(int id)
        {
            return _pageSectionRepository.GetById(id);
        }

        public void AddPageSection(PageSection pageSection)
        {
            _pageSectionRepository.Add(pageSection);
        }

        public void UpdatePageSection(PageSection pageSection)
        {
            _pageSectionRepository.Update(pageSection);
        }

        public void DeletePageSection(int pageSectionId)
        {
            PageSection pageSection = _pageSectionRepository.GetById(pageSectionId);
            if (pageSection != null)
            {
                pageSection.Excluido = true;
                pageSection.Ativo = false;

                _pageSectionRepository.Update(pageSection);
            }
        }
    }

}
