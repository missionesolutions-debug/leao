using Data;
using Data.Models.Core;
using Framework.Repositories;
using Framework.Services.ApplicationServices.Core;

namespace Framework.Services.ApplicationServices.Configs
{
    public class ConfigsService : GenericService<HeadConfig, IRepository<HeadConfig>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemService _srvItem;
        private readonly IRepository<HeadConfig> _repository;

        public ConfigsService(ApplicationDbContext context, ItemService itemService, IRepository<HeadConfig> repository) : base(context, itemService, repository)
        {
            _context = context;
            _srvItem = itemService;
            _repository = repository;
        }
    }
}
