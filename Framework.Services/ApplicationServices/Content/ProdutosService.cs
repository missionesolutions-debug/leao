using Data;
using Data.Models.Catalogo;
using Framework.Repositories;
using Framework.Services.ApplicationServices.Core;

namespace Framework.Services.ApplicationServices.Content
{
    public class ProdutosService : GenericService<Produto, IRepository<Produto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemService _srvItem;
        private readonly IRepository<Produto> _repository;

        public ProdutosService(ApplicationDbContext context, ItemService itemService, IRepository<Produto> repository) : base(context, itemService, repository)
        {
            _context = context;
            _srvItem = itemService;
            _repository = repository;
        }
    }
}
