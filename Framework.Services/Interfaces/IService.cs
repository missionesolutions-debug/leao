using Framework.Domain.Site.Component;
using System.Collections.Generic;

namespace Framework.Services.Site.Interfaces
{
    public interface IService
    {
        public List<Item> ListItem();
        public Item GetItemById(int id);
		public Item GetItemByUrl(string url);
	}
}
