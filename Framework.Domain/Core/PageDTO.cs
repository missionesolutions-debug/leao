using Framework.Domain.Site.Component;
using Framework.Domain.Site.Interfaces;
using System.Collections.Generic;

namespace Framework.Domain.Site.Core
{
    public class PageDTO : IPage
    {
        public Item Page { get; set; }
		public List<Item> Artigos { get; set; }
        public List<Item> Categorias { get; set; }
        public List<Item> Banners { get; set; }
        public List<Item> Posts { get; set; }
        public List<Item> Equipes { get; set; }
        public List<Item> Servicos { get; set; }
        public List<Item> Depoimentos { get; set; }
        public List<Item> Cursos { get; set; }
        public List<Item> Marcas { get; set; }
        public List<Item> Produtos { get; set; }
        public List<Item> Products { get; set; }
        public List<Item> Clientes { get; set; }
        public List<Item> Fotos { get; set; }
        public List<Item> Sections { get; set; } = new List<Item>();

    }
}
