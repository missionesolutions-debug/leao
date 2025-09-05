namespace Framework.Domain.Models.Configs
{
    public class SitemapEntity
    {
        public string EntityName { get; set; }
        public string EntityRoute { get; set; }
        public decimal Priority { get; set; }
        public bool IsActive { get; set; }
    }
}
