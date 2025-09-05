namespace Framework.Domain.Site.Interfaces
{
    public interface IHead
    {
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string ImageOpenGraph { get; set; }
        public string HeadScripts { get; set; }
    }
}
