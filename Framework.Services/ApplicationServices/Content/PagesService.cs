using Data.Models.Conteudo;
using Data;
using Framework.Repositories;
using Framework.Data.Models.Core;
using Framework.Domain.Site.Core;
using System.Linq;
using System.Collections.Generic;
using Framework.Domain.Site.Component;
using Data.Models.Catalogo;
using Data.Models.Pessoa;
using Framework.Domain.Core;
using Framework.Factories.Core;
using Framework.Services.ApplicationServices.Core;
using Framework.Services.Interfaces.Core;
using Data.Models.Ecommerce;
using PagedList.Core;
using Data.Models.Propriedade;
using System.Data.Entity.Core;

namespace Framework.Services.ApplicationServices.Content
{
    public class PagesService : GenericService<Pagina, IRepository<Pagina>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemService _srvItem;
        private readonly IRepository<Pagina> _repository;
        private readonly IRepository<PaginaContent> _paginaContentRepository;
        private readonly ImagemFactory _imagemRepository;
        private readonly Dictionary<string, Func<string, int, string, string, List<Item>>> _contentBuilders;
        private readonly IPageSectionService _pageSectionService;
        private readonly IPageSectionItemService _pageSectionItemService;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Servico> _servicoRepository;
        private readonly IRepository<Artigo> _artigoRepository;
        private readonly IRepository<Produto> _produtoRepository;
        private readonly IRepository<Foto> _fotoRepository;
        private readonly IRepository<Curso> _cursoRepository;
		private readonly IRepository<Categoria> _categoriaRepository;
        public PagesService(ApplicationDbContext context,
            ItemService itemService,
            IRepository<Pagina> repository,
            IRepository<PaginaContent> paginaContentRepository,
            IRepository<Banner> bannerRepository,
			  IRepository<Categoria> categoriaRepository,
			IRepository<Equipe> equipeRepository,
            IRepository<Cliente> clienteRepository,
            IRepository<Marca> marcaRepository,
            IRepository<Depoimento> depoimentoRepository,
            IRepository<Artigo> artigoRepository,
            IRepository<Blog> blogRepository,
            IRepository<Product> productRepository,
            ImagemFactory imagemRepository,
            IPageSectionService pageSectionService,
            IPageSectionItemService pageSectionItemService,
            IRepository<Servico> servicoRepository,
            IRepository<Produto> produtoRepository,
            IRepository<Foto> fotoRepository,
            IRepository<Curso> cursoRepository
            )
            : base(context, itemService, repository)
        {
            _context = context;
            _srvItem = itemService;
            _repository = repository;
            _paginaContentRepository = paginaContentRepository;
            _imagemRepository = imagemRepository;
            _pageSectionService = pageSectionService;
            _pageSectionItemService = pageSectionItemService;
            _productRepository = productRepository;
            _servicoRepository = servicoRepository;
            _artigoRepository = artigoRepository;
            _productRepository = productRepository;
            _produtoRepository = produtoRepository;
            _fotoRepository = fotoRepository;
            _cursoRepository = cursoRepository;
			_categoriaRepository = categoriaRepository;

			_contentBuilders = new Dictionary<string, Func<string, int, string, string, List<Item>>>
            {
                { "Artigo", (chave, paginaId, selector,ignoreFields) => BuildContent(artigoRepository, chave, paginaId, selector, ignoreFields) },
				{ "Servico", (chave, paginaId, selector,ignoreFields) => BuildContent(servicoRepository, chave, paginaId, selector, ignoreFields) },
				{ "Banner", (chave, paginaId, selector, ignoreFields) => BuildContent(bannerRepository, chave, paginaId, selector, ignoreFields) },
                { "Blog", (chave, paginaId, selector, ignoreFields) => BuildContent(blogRepository, chave, paginaId, selector, ignoreFields) },
                { "Depoimento", (chave, paginaId, selector, ignoreFields) => BuildContent(depoimentoRepository, chave, paginaId, selector, ignoreFields) },
                { "Equipe", (chave, paginaId, selector, ignoreFields) => BuildContent(equipeRepository, chave, paginaId, selector, ignoreFields) },
                { "Marca", (chave, paginaId, selector, ignoreFields) => BuildContent(marcaRepository, chave, paginaId, selector, ignoreFields) },
                { "Cliente", (chave, paginaId, selector, ignoreFields) => BuildContent(clienteRepository, chave, paginaId, selector, ignoreFields) },
                { "Product", (chave, paginaId, selector, ignoreFields) => BuildContent(productRepository, chave, paginaId, selector, ignoreFields) },
                { "Produto", (chave, paginaId, selector, ignoreFields) => BuildContent(produtoRepository, chave, paginaId, selector, ignoreFields) },
                { "Foto", (chave, paginaId, selector, ignoreFields) => BuildContent(fotoRepository, chave, paginaId, selector, ignoreFields) },
                { "Curso", (chave, paginaId, selector, ignoreFields) => BuildContent(cursoRepository, chave, paginaId, selector, ignoreFields) }
            };
        }
        public PageDTO BuildPage(string url)
        {
            PageDTO dto = new PageDTO();

            try
            {
                Pagina pagina = _repository.GetByUrl(url);

                if (pagina is null)
                    throw new EntityException("Page not found");

                dto.Page = _srvItem.Build(pagina);

                var contents = _paginaContentRepository.GetAll()
                    .Where(b => b.PaginaId == pagina.Id)
                    .ToList();

                foreach (var item in contents)
                {
                    if (_contentBuilders.TryGetValue(item.TableAction, out var builder))
                    {
                        // Passe um valor adequado para o selector conforme sua lógica
                        var content = builder(item.Chave, item.PaginaId, item.Selector, item.IgnoreFields);

                        switch (item.TableAction)
                        {
                            case "Artigo":
                                dto.Artigos = content;
                                break;
                            case "Banner":
                                dto.Banners = content;
                                break;
                            case "Blog":
                                dto.Posts = content;
                                break;
                            case "Depoimento":
                                dto.Depoimentos = content;
                                break;
                            case "Equipe":
                                dto.Equipes = content;
                                break;
                            case "Marca":
                                dto.Marcas = content;
                                break;
                            case "Cliente":
                                dto.Clientes = content;
                                break;
                            case "Produto":
                                dto.Produtos = content;
                                break;
							case "Servico":
								dto.Servicos = content;
								break;
							case "Product":
                                dto.Products = content;
                                break;
                            case "Foto":
                                dto.Fotos = content;
                                break;
                            case "Curso":
                                dto.Cursos = content;
                                break;
                        }
                    }
                }

                dto.Sections = GetPageSections(pagina.Id, "Pagina");


            }
            catch (Exception ex)
            {

            }

            return dto;

        }


        public ListPageDTO GetGroup<T>(IRepository<T> repository, string category, string search, string tags, int? page, bool? destaque, bool? destaqueVitrine, int pageSize = 12) where T : class
        {
            List<Categoria> categories = _categoriaRepository.GetAllAtivo().ToList();

            if (!search.IsNullOrEmpty())
                search = search.ToLower();

            #region Initialize
            ListPageDTO dto = new ListPageDTO();
            int Pagesize = pageSize;
            int PageNumber = page == 0 ? 1 : page ?? 1;
			#endregion

			#region Filter Category
			IEnumerable<T> entities;

			switch (category)
			{
				case null:
				case "GetAll":
					entities = repository.GetAllAtivo();
					break;

				default:
					entities = repository.GetAllByCategoriaUrl(category);
					break;
			}
			#endregion

			#region Search
			if (!string.IsNullOrEmpty(search))
                entities = entities.Where(e => typeof(T).GetProperty("Titulo")?.GetValue(e)?.ToString()?.ToLower().Contains(search) == true ||
                                               typeof(T).GetProperty("Descricao")?.GetValue(e)?.ToString()?.ToLower().Contains(search) == true);
            #endregion

            #region Destaque e DestaqueVitrine
            if (destaque == true)
            {
                var destaqueProperty = typeof(T).GetProperty("Destaque");

                if (destaqueProperty != null)
                {
                    entities = entities.Where(e =>
                        destaqueProperty.GetValue(e) is bool value && value);
                }
            }
            if (destaqueVitrine == true)
            {
                var destaqueVitrineProperty = typeof(T).GetProperty("DestaqueVitrine");

                if (destaqueVitrineProperty != null)
                {
                    entities = entities.Where(e =>
                        destaqueVitrineProperty.GetValue(e) is bool value && value);
                }
            }
            #endregion

            var allTags = entities
                 .Where(entity => typeof(T).GetProperty("Descricao")?.GetValue(entity) != null)
                 .SelectMany(entity => typeof(T).GetProperty("Descricao")?.GetValue(entity)?.ToString().Split(';') ?? Array.Empty<string>())
                 .Distinct()
                 .ToArray();

            dto.Page.PageTitle = "Group";
            dto.Page.Tags = string.Join(";", allTags);

            var categorias = entities
                                     .Select(e => typeof(T).GetProperty("Categoria")?.GetValue(e))
                                     .Where(c => c != null)
                                     .Distinct()
                                     .ToList();

            foreach (Categoria item in categorias)
            {
                dto.Categorias.Add(new Item()
                {
                    Titulo = item.Titulo,
                    Url = item.Url,
                });
            }

            entities = entities.OrderByDescending(e => (int?)typeof(T).GetProperty("Id")?.GetValue(e)).ToList();

            #region Creator
            IQueryable<Item> EntityPosts = Creator(entities.ToList());
            #endregion

            #region Pagination
            dto.Pages = EntityPosts.ToPagedList(PageNumber, Pagesize);

            Pagination pagination = new Pagination
            {
                PageSize = Pagesize,
                PageTotal = entities.Count(),
                PageNumber = dto.Pages.PageNumber,
                IsLastPage = dto.Pages.IsLastPage,
                IsFirstPage = dto.Pages.IsFirstPage,
                HasPreviousPage = dto.Pages.HasPreviousPage,
                HasNextPage = dto.Pages.HasNextPage,
                StartPage = 1,
                totalPages = dto.Pages.PageCount,
                totalItems = dto.Pages.Count,
                EndPage = dto.Pages.PageCount,
                pages = Enumerable.Range(1, dto.Pages.PageCount).ToArray()
            };

            dto.Pagination = pagination;
            #endregion

            return dto;
        }

        private IQueryable<Item> Creator<T>(List<T> entities) where T : class
        {
            List<Item> list = new List<Item>();

            foreach (var entity in entities)
            {
                list.Add(_srvItem.Build(entity));
            }

            return list.AsQueryable();
        }

        public ListPageDTO GetItems(string entityKey, string category, string search, string tags, int? page, bool? destaque, bool? destaqueVitrine, int pageSize = 32)
        {
            switch (entityKey)
            {
				case "categories":
					return GetGroup(_categoriaRepository, category, search, tags, page, destaque, destaqueVitrine, pageSize);
				case "products":
                    return GetGroup(_productRepository, category, search, tags, page, destaque, destaqueVitrine, pageSize);
                case "produtos":
                    return GetGroup(_produtoRepository, category, search, tags, page, destaque, destaqueVitrine, pageSize);
                case "services":
                    return GetGroup(_servicoRepository, category, search, tags, page, destaque, destaqueVitrine);
                case "artigos":
                    return GetGroup(_servicoRepository, category, search, tags, page, destaque, destaqueVitrine);
                default:
                    return new ListPageDTO();
            }
        }


        #region Groups

        public List<Item> GetPageSections(int Id, string OwnerType)
        {
            // Fetch PageSections
            var pageSections = _pageSectionService.GetPageSectionsByOwner(Id, OwnerType);

            var pageSectionItems = new List<Item>();

            foreach (var section in pageSections)
            {
                var sectionItem = new Item
                {
                    Ref = section.Ref,
                    Titulo = section.Titulo,
                    Subtitulo = section.Subtitulo,
                    Descricao = section.Descricao,
                    Imagem = section.Imagem,
                    Arquivo = section.Arquivo,
                    Thumbnail = section.Thumbnail
                };

                // Fetch PageSectionItems
                var items = _pageSectionItemService.GetPageSectionItems(section.Id);

                foreach (var item in items)
                {
                    var itemObj = new Item
                    {
                        Ref = item.Ref,
                        Imagem = item.Imagem,
                        Titulo = item.Titulo,
                        Subtitulo = item.Subtitulo,
                        Descricao = item.Descricao,
                        Arquivo = item.Arquivo,
                        Thumbnail = section.Thumbnail
                    };

                    sectionItem.Items.Add(itemObj);
                }

                pageSectionItems.Add(sectionItem);
            }

            return pageSectionItems;
        }

     
        //public ListPageDTO GetGroupPage(string url)
        //{
        //    ListPageDTO dto = new ListPageDTO();

        //    foreach (var item in _repository.GetAllAtivo().Where(b => b.GroupPagina == url).ToList())
        //    {
        //        dto.Pages.Add(new PageItemDTO()
        //        {
        //            Id = item.Id,
        //            Link = item.Url,
        //            Ordem = item.Ordem,
        //            Titulo = item.Titulo
        //        });
        //    };

        //    return dto;
        //}

        private List<Item> BuildContent<T>(IRepository<T> repository, string chave, int paginaId, string selector, string ignoreFields = "") where T : class
        {
            var dto = new List<Item>();
            var items = repository.GetAllAtivo().ToList();

            foreach (var item in items)
            {
                var itemType = item.GetType();
                bool isAbleToAdd = false;

                if (!string.IsNullOrEmpty(selector) && selector == "GetAll")
                {
                    isAbleToAdd = true;
                }
                else if (!string.IsNullOrEmpty(selector) && selector == "GetAllWithKey" && !string.IsNullOrEmpty(chave))
                {
                    var chaveProperty = itemType.GetProperty("Chave");
                    if (chaveProperty != null)
                    {
                        var itemChave = chaveProperty?.GetValue(item)?.ToString();

                        if (itemChave == chave)
                            isAbleToAdd = true;
                    }
                }
                else if (!string.IsNullOrEmpty(selector) && selector == "GetByPaginaId" && paginaId > 0)
                {
                    var paginaIdProperty = itemType.GetProperty("PaginaId");

                    if (paginaIdProperty != null)
                    {
                        var itemPaginaId = (int?)paginaIdProperty?.GetValue(item);

                        if (itemPaginaId > 0 && itemPaginaId == paginaId)
                            isAbleToAdd = true;
                    }
                }
                else if (!string.IsNullOrEmpty(selector) && selector == "GetByPaginaIdAndChave" && !string.IsNullOrEmpty(chave) && paginaId > 0)
                {
                    var paginaIdProperty = itemType.GetProperty("PaginaId");

                    if (paginaIdProperty != null)
                    {
                        var itemPaginaId = (int?)paginaIdProperty?.GetValue(item);

                        if (itemPaginaId > 0 && itemPaginaId == paginaId)
                        {
                            var chaveProperty = itemType.GetProperty("Chave");
                            if (chaveProperty != null)
                            {
                                var itemChave = chaveProperty?.GetValue(item)?.ToString();

                                if (itemChave == chave)
                                    isAbleToAdd = true;
                            }
                        }
                    }
                }

                if (isAbleToAdd)
                {
                    Item obj = _srvItem.Build(item, ignoreFields.IsNullOrEmpty() ? null : ignoreFields.Split(';').ToList());

                    Type itemTypeT = typeof(T);

                    try
                    {
                        var Imagens = _imagemRepository.GetImagemsByTableActionAndTableId(itemTypeT.Name.ToString() + "s", obj.Id.Value);

                        foreach (var imagem in Imagens)
                        {
                            obj.imagens.Add(_srvItem.Build(imagem));
                        }
                    }
                    catch (Exception ex)
                    {
                    }


                    dto.Add(obj);

                }
            }

            return dto;
        }

        public PageDTO GetDetailGroup(string entityKey, string url)
        {
            PageDTO dto = new PageDTO();

            switch (entityKey)
            {
                case "cursos":
                    Curso curso = _cursoRepository.GetByUrl(url);
                    dto.Page = _srvItem.Build(curso);
                    dto.Sections = GetPageSections(curso.Id, "Cursos");
                    break;
                case "products":
                    Product product = _productRepository.GetByUrl(url);
                    dto.Page = _srvItem.Build(product);
                    dto.Sections = GetPageSections(product.Id, "Products");
                    break;

                case "produtos":
                    Produto produto = _produtoRepository.GetByUrl(url);
                    dto.Page = _srvItem.Build(produto);
                    dto.Sections = GetPageSections(produto.Id, "Produtos");
                    break;
                case "services":
					Servico servico = _servicoRepository.GetByUrl(url);
					dto.Page = _srvItem.Build(servico);
					dto.Sections.AddRange(GetPageSections(servico.Id, "Servicos"));
					dto.Sections.AddRange(GetPageSections(servico.Id, "Servico"));
					break;
                case "artigos": dto.Page = _srvItem.Build(_artigoRepository.GetByUrl(url)); break;

                default:
                    break;
            }

            return dto;
        }

        #endregion
  
    }
}
