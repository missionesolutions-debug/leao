using Framework.Data.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Services.Interfaces.Configs
{
    public interface ISitemapService
    {
        Task<List<SitemapUrl>> GetSitemapAsync();
        string ConvertSitemapToXml(List<SitemapUrl> sitemapUrls);
    }

}
