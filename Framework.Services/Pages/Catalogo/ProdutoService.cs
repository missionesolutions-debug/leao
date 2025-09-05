//using Data;
//using Data.Models.Catalogo;
//using Data.Models.Propriedade;
//using Framework.Domain.Site.Component;
//using Framework.Domain.Site.Pages.Catalogo;
//using Framework.Factories.Conteudo;
//using Framework.Factories.Propriedade;
//using Framework.Services.Site.Helpers;
//using Framework.Services.Site.Interfaces;
//using PagedList.Core;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Core;
//using System.Linq;
//using Framework.Tools;

//namespace Framework.Services.Site.Pages.Catalogo
//{
//    public class ProdutoService : IService
//    {
//        private static ApplicationDbContext _context;
//        private static ItemService _srvItem;
//        private static ProdutoFactory _faProduto;
//        private static HeadService _srvHead;
//        private static BodyService _srvBody;
//        private static CategoriaFactory _faCategoria;
//        private static BadgesFactory _faBadges;

//        public ProdutoService(ApplicationDbContext context)
//        {
//            _context = context;
//            _srvItem = new ItemService(context);
//            _faProduto = new ProdutoFactory(context);
//            _srvHead = new HeadService();
//            _srvBody = new BodyService();
//            _faCategoria = new CategoriaFactory(context);
//            _faBadges = new BadgesFactory(context);
//        }

//        public ProdutoPage List(string category, string search, string tags, int? page)
//        {
//            List<Produto> Produtos = new List<Produto>();

//            ProdutoPage model = new ProdutoPage();

//            int Pagesize = 130;
//            int PageNumber = page == 0 ? 1 : page != null ? page.ToInt() : 1;

//            List<Categoria> Categorias = _faCategoria.GetAllAtivo();

//            try
//            {
//                #region Filter Category
//                if (string.IsNullOrEmpty(category))
//                {
//                    Produtos = _faProduto.GetAllAtivoAndCategoryActive().Where(b => b.Id != 47).ToList();
//                }

//                if (!string.IsNullOrEmpty(category))
//                {
//                    Produtos = _faProduto.GetAllAtivoByCategory(category).Where(b => b.Id != 47).ToList();
//                }
//                if (category == "GetAll")
//                {
//                    Produtos = _faProduto.GetAllAtivo().Where(b => b.Id != 47).ToList();
//                }

//                #endregion

//                #region Search
//                if (!string.IsNullOrEmpty(search))
//                {
//                    Produtos = Produtos.Where(b => b.Titulo != null && b.Titulo.ToLower().Contains(search)).ToList();
//                }
//                #endregion

//                #region Pagination
//                IQueryable<Item> ProdutoPosts = Creator(Produtos);
//                model.Items = ProdutoPosts.ToPagedList(PageNumber, Pagesize);

//                model.PageSize = Pagesize;
//                model.PageTotal = Produtos.Count();
//                model.PageNumber = model.Items.PageNumber;
//                model.IsLastPage = model.Items.IsLastPage;
//                model.IsFirstPage = model.Items.IsFirstPage;
//                model.HasPreviousPage = model.Items.HasPreviousPage;
//                model.HasNextPage = model.Items.HasNextPage;
//                #endregion
//            }
//            catch (EntityException ex)
//            {
//                throw new Exception("500InternalServerError", new Exception());
//            }

//            return model;
//        }
//        public ProdutoDetailPage Detail(string url)
//        {
//            Produto Produto = _faProduto.GetByUrl(url);

//            if (Produto != null)
//            {
//                ProdutoDetailPage model = new ProdutoDetailPage
//                {
//                    Detail = _srvItem.BuildDetail(Produto),
//                    Head = _srvHead.Build(Produto),
//                    Body = _srvBody.Build(Produto),
//			};

//                return model;
//            }

//            throw new Exception("404NotFound", new Exception());

//        }
//        public List<Item> ListItemByCategoriaUrl(string url)
//        {
//            try
//            {
//                List<Item> Items = new List<Item>();

//                if (url.IsNullOrEmpty())
//                    throw new Exception("404NotFound", new Exception());

//                if (url == "GetAll")
//                    foreach (var item in _faProduto.GetAllAtivo().OrderByDescending(b => b.Ordem))
//                    {
//                        Items.Add(_srvItem.Build(item));
//                    }
//                else
//                    foreach (var item in _faProduto.GetProdutosByCategoriaUrl(url).OrderByDescending(b => b.Ordem))
//                    {
//                        Items.Add(_srvItem.Build(item));
//                    }

//                return Items;
//            }
//            catch (Exception)
//            {
//                throw new Exception("500InternalServerError", new Exception());
//            }
//        }

//        #region Methods Private
//        private IQueryable<Item> Creator(List<Produto> Produtos)
//        {
//            List<Item> List = new();

//            foreach (var item in Produtos)
//            {
//                List.Add(_srvItem.Build(item));
//            }

//            return List.AsQueryable();
//        }

//        #endregion

//        #region Unused
//        public List<Item> ListItem()
//        {
//            try
//            {
//                List<Item> Items = new List<Item>();

//                foreach (var item in _faProduto.GetAllAtivo().OrderByDescending(b => b.Ordem))
//                {
//                    Items.Add(_srvItem.Build(item));
//                }

//                return Items;
//            }
//            catch (Exception)
//            {
//                throw new Exception("500InternalServerError", new Exception());
//            }

//        }
//        public Item GetItem(string url)
//        {
//            try
//            {
//                if (url.IsNullOrEmpty())
//                    throw new Exception("404NotFound", new Exception());

//                return _srvItem.Build(_faProduto.GetObjByUrl(url));

//            }
//            catch (Exception)
//            {
//                throw new Exception("500InternalServerError", new Exception());
//            }
//        }
//        public Item GetItem(int id)
//        {
//            try
//            {
//                if (id > 0)
//                    return _srvItem.Build(_faProduto.GetObj(id));

//                throw new Exception("404NotFound", new Exception());
//            }
//            catch (Exception)
//            {
//                throw new Exception("500InternalServerError", new Exception());
//            }
//        }
//        #endregion


//    }
//}
