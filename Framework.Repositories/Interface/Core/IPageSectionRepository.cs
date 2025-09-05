using Data;
using Framework.Data.Models.Conteudo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories.Interface.Core
{
    public interface IPageSectionRepository : IRepository<PageSection>
    {
        IEnumerable<PageSection> GetPageSectionsByOwner(int ownerId, string ownerType);
    }

    public class PageSectionRepository : Repository<PageSection>, IPageSectionRepository
    {
        public PageSectionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<PageSection> GetPageSectionsByOwner(int ownerId, string ownerType)
        {
            return _context.PageSection
                .Where(ps => ps.OwnerId == ownerId && ps.OwnerType == ownerType && !ps.Excluido)
                .Include(ps => ps.PageSectionItems)
                //.Include(ps => ps.StyleProperties)
                .OrderBy(ps => ps.Ordem)
                .ToList();
        }
    }

}
