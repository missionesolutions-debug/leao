using Framework.Domain.Site.Component;
using PagedList.Core;
using System.Collections.Generic;

namespace Framework.Domain.Core
{
    public class ListPageDTO
    {
        public Item Page { get; set; } = new Item();
        public IPagedList<Item> Pages { get; set; }
        public List<Item> Categorias { get; set; } = new List<Item>();
        public Pagination Pagination { get; set; } = new Pagination();
    }

    public class PageItemDTO
    {
        public int Id { get; set; }
        public int? Ordem { get; set; }
        public string Titulo { get; set; }
        public string Link { get; set; }
        public string GroupPage { get; set; }
    }
}
