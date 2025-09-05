using Framework.Data.Models.Common;
using Framework.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Configs
{
    public class SitemapUnitOfWorkService
    {
        //public readonly IProdutoRepository _produtoRepository;
        public readonly IServicoRepository _servicoRepository;
        public readonly IBlogRepository _blogRepository;
        //public readonly IArtigoRepository _artigoRepository;
        //public readonly IFotoRepository _fotoRepository;
        public readonly IPaginaRepository _paginaRepository;
        public SitemapUnitOfWorkService(
            //IProdutoRepository produtoRepository,
            IServicoRepository servicoRepository,
            IBlogRepository blogRepository,
            //IArtigoRepository artigoRepository,
            //IFotoRepository fotoRepository,
            IPaginaRepository paginaRepository
            )
        {
            //_produtoRepository = produtoRepository;
            _servicoRepository = servicoRepository;
            _blogRepository = blogRepository;
            //_artigoRepository = artigoRepository;
            //_fotoRepository = fotoRepository;
            _paginaRepository = paginaRepository;
            // Inicialize outros repositórios conforme necessário
        }

        //public async Task<IEnumerable<SitemapItem>> GetProdutosAsync()
        //{
        //    var produtos = await _produtoRepository.GetAllActiveAsync();
        //    return produtos.Select(p => new SitemapItem
        //    {
        //        Url = p.Url,
        //        LastModified = p.LastModified
        //    });
        //}

        public async Task<IEnumerable<SitemapItem>> GetServicosAsync()
        {
            var servicos = await _servicoRepository.GetActivePagesAsync();
            return servicos.Select(s => new SitemapItem
            {
                Url = s.Url,
                LastModified = s.DataEdicao
            });
        }

        public async Task<IEnumerable<SitemapItem>> GetBlogsAsync()
        {
            var blogs = await _blogRepository.GetActivePagesAsync();
            return blogs.Select(b => new SitemapItem
            {
                Url = b.Url,
                LastModified = b.DataEdicao
            });
        }

        //public async Task<IEnumerable<SitemapItem>> GetArtigosAsync()
        //{
        //    var artigos = await _artigoRepository.GetAllActiveAsync();
        //    return artigos.Select(a => new SitemapItem
        //    {
        //        Url = a.Url,
        //        LastModified = a.LastModified
        //    });
        //}

        //public async Task<IEnumerable<SitemapItem>> GetFotosAsync()
        //{
        //    var fotos = await _fotoRepository.GetAllActiveAsync();
        //    return fotos.Select(f => new SitemapItem
        //    {
        //        Url = f.Url,
        //        LastModified = f.LastModified
        //    });
        //}

        public async Task<IEnumerable<SitemapItem>> GetPaginasAsync()
        {
            var paginas = await _paginaRepository.GetActivePagesAsync();
            return paginas.Select(p => new SitemapItem
            {
                Url = p.Url,
                LastModified = p.DataEdicao
            });
        }

    }
}
