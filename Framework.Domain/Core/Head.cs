using Framework.Domain.Site.Interfaces;

namespace Framework.Domain.Site.Core
{
    public class Head : IHead
    {
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string ImageOpenGraph { get; set; }
        public string HeadScripts { get; set; }
    }
}
