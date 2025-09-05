using Framework.Data.Models.Common;
using Framework.Domain.Models.Configs;
using Framework.Factories.System;
using Framework.Services.Interfaces.Configs;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Configs
{
    public class SitemapService : ISitemapService
    {
        private readonly SitemapSettings _sitemapSettings;

        private readonly PageFactory _pageRepository;
        private readonly SitemapUnitOfWorkService _sitemapUnitOfWorkService;


        public SitemapService(
            PageFactory pageRepository,
            IOptions<SitemapSettings> sitemapSettings,
            SitemapUnitOfWorkService sitemapUnitOfWorkService
            )
        {
            _pageRepository = pageRepository;
            _sitemapSettings = sitemapSettings.Value;
            _sitemapUnitOfWorkService = sitemapUnitOfWorkService;
        }

        public async Task<List<SitemapUrl>> GetSitemapAsync()
        {
            var sitemapUrls = new List<SitemapUrl>();

            // Obter as páginas que estão ativas e habilitadas para o sitemap
            var activePages = await _pageRepository.GetPagesForSitemapAsync();

            foreach (var page in activePages)
            {
                string entityName = page.EntityName;
                string entityRoute = page.EntityRoute;
                string priority = page.Priority;

                IEnumerable<SitemapItem> items = await GetItemsForEntityAsync(entityName);

                if (items == null)
                    continue;

                sitemapUrls.AddRange(BuildUrls(entityRoute, items, priority));
            }

            return sitemapUrls.OrderByDescending(b => b.Priority).ToList();
        }

        private async Task<IEnumerable<SitemapItem>> GetItemsForEntityAsync(string entityName)
        {
            switch (entityName)
            {
                //case "Produto":
                //return await GetProdutosAsync();
                case "Servico":
                    return await _sitemapUnitOfWorkService.GetServicosAsync();
                case "Blog":
                    return await _sitemapUnitOfWorkService.GetBlogsAsync();
                //case "Artigo":
                //return await GetArtigosAsync();
                //case "Foto":
                //return await GetFotosAsync();
                case "Pagina":
                    return await _sitemapUnitOfWorkService.GetPaginasAsync();
                // Adicione outros casos conforme necessário
                default:
                    return null;
            }
        }
       
        private List<SitemapUrl> BuildUrls(string entityRoute, IEnumerable<SitemapItem> items, string priority)
        {
            var urls = new List<SitemapUrl>();
            foreach (var item in items)
            {
                var url = new SitemapUrl
                {
                    Loc = GetLocation(entityRoute,item.Url),
                    LastMod = item.LastModified,
                    ChangeFreq = "weekly",
                    Priority = priority
                };
                urls.Add(url);
            }
            return urls;
        }

        public string GetLocation(string entityRoute, string url)
        {

            if (entityRoute.Length > 0)
                return $"{_sitemapSettings.Domain}/{entityRoute}/{url}";

            return $"{_sitemapSettings.Domain}/{url}";

        }

        public string ConvertSitemapToXml(List<SitemapUrl> sitemapUrls)
        {
            var xmlSitemap = new System.Text.StringBuilder();
            xmlSitemap.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            xmlSitemap.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            foreach (var url in sitemapUrls)
            {
                xmlSitemap.AppendLine("  <url>");
                xmlSitemap.AppendLine($"    <loc>{url.Loc}</loc>");
                if (url.LastMod != null)
                    xmlSitemap.AppendLine($"    <lastmod>{url.LastMod.Value.ToString("yyyy-MM-dd")}</lastmod>");
                if (!string.IsNullOrEmpty(url.ChangeFreq))
                    xmlSitemap.AppendLine($"    <changefreq>{url.ChangeFreq}</changefreq>");
                if (url.Priority != null)
                    xmlSitemap.AppendLine($"    <priority>{url.Priority}</priority>");
                xmlSitemap.AppendLine("  </url>");
            }

            xmlSitemap.AppendLine("</urlset>");

            return xmlSitemap.ToString();
        }
    }
}
