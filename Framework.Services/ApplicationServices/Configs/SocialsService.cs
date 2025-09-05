using Data;
using Data.Models.Core;
using Framework.Repositories;
using Framework.Services.ApplicationServices.Core;

namespace Framework.Services.ApplicationServices.Configs
{
    public class SocialsService : GenericService<RedesContato, IRepository<RedesContato>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemService _srvItem;
        private readonly IRepository<RedesContato> _repository;

        public SocialsService(ApplicationDbContext context, ItemService itemService, IRepository<RedesContato> repository) : base(context, itemService, repository)
        {
            _context = context;
            _srvItem = itemService;
            _repository = repository;
        }




    }
}
