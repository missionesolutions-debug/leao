using Data;
using Framework.Data.Models.Conteudo;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories.Interface.Core
{
    public interface IPageSectionItemRepository : IRepository<PageSectionItem>
    {
        IEnumerable<PageSectionItem> GetPageSectionItemsByPageSectionId(int pageSectionId);
    }

    public class PageSectionItemRepository : Repository<PageSectionItem>, IPageSectionItemRepository
    {
        public PageSectionItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<PageSectionItem> GetPageSectionItemsByPageSectionId(int pageSectionId)
        {
            return _context.PageSectionItem
                .Where(psi => psi.PageSectionId == pageSectionId && !psi.Excluido)
                //.Include(psi => psi.StyleProperties)
                .OrderBy(psi => psi.Ordem)
                .ToList();
        }
    }

}
