using Data;
using Data.Models.PainelConfig;

namespace Framework.Repositories.Core
{
    public class PageRepository : Repository<Page>
    {
        private readonly ApplicationDbContext _context;

        public override Repository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
