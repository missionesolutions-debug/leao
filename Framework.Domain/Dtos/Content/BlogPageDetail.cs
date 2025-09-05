using Framework.Domain.Site.Component;
using System.Collections.Generic;

namespace Framework.Domain.Dtos.Content
{
    public class BlogPageDetail
    {
        public Item Page { get; set; }
        public List<Item> Posts { get; set; } = new List<Item>();
        public List<Item> Categorias { get; set; } = new List<Item>();
    }
}
