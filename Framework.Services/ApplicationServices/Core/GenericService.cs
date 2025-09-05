using Data;
using Framework.Domain.Site.Component;
using Framework.Repositories;
using Framework.Services.Site.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Services.ApplicationServices.Core
{
    public class GenericService<T, R> : IService where R : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemService _srvItem;
        private readonly R _repository;

        public GenericService(ApplicationDbContext context, ItemService itemService, R repository)
        {
            _context = context;
            _srvItem = itemService;
            _repository = repository;
        }

        public List<Item> ListItem()
        {
            try
            {
                var items = _repository.GetAllAtivo();
                return items.Select(item => _srvItem.Build(item)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving items", ex);
            }
        }

        public Item GetItemById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID must be greater than zero", nameof(id));
                }

                var item = _repository.GetObj(id);
                return _srvItem.Build(item);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the item by ID", ex);
            }
        }

        public Item GetItemByUrl(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("URL cannot be null or empty", nameof(url));
                }

                var item = _repository.GetByUrl(url);

                return _srvItem.Build(item);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the item by URL", ex);
            }
        }

    }
}
