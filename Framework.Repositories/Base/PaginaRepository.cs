using Data;
using Data.Models.Conteudo;
using Framework.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Repositories.Base
{
    public class PaginaRepository : IPaginaRepository
    {
        private readonly ApplicationDbContext _context;

        public PaginaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Pagina>> GetActivePagesAsync()
        {
            var items = new List<Pagina>();
            var result = _context.Pagina.Where(b => b.Ativo == true).ToList();
            return Task.FromResult((IEnumerable<Pagina>)result);
        }
    }
}
