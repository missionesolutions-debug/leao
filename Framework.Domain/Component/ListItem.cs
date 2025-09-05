using Framework.Domain.Site.Core;

namespace Framework.Domain.Site.Component
{
    public class ListItem
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string Imagem { get; set; }
        public string Url { get; set; }
        public string Categoria { get; set; }
        public int PageTotal { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
        public Head Head { get; set; }
        public Body Body { get; set; }
    }
}
