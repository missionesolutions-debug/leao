using System.Collections.Generic;

namespace Framework.Domain.Models.Configs
{
    public class SitemapSettings
    {
        public string Domain { get; set; }
        public List<SitemapEntity> Entities { get; set; }
    }
}
