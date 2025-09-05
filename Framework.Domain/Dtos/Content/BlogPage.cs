using Framework.Domain.Core;
using Framework.Domain.Site.Component;
using Framework.Domain.Site.Core;
using PagedList.Core;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Content
{
    public class BlogPage
    {

        public Head Head { get; set; }
        public Body Body { get; set; }
        public Item Page { get; set; } = new Item();
        public IPagedList<Item> Posts { get; set; }
        public List<Item> Destaques { get; set; } = new List<Item>();
        public List<Item> Categorias { get; set; } = new List<Item>();
        public Pagination Pagination { get; set; } = new Pagination();

    }
}
