//using Data;
//using Data.Models.Conteudo;
//using Data.Models.Propriedade;
//using Framework.Domain.Site.Component;
//using Framework.Domain.Site.Pages.Conteudo;
//using Framework.Factories.Conteudo;
//using Framework.Factories.Core;
//using Framework.Factories.Propriedade;
//using Framework.Services.Site.Helpers;
//using Framework.Services.Site.Interfaces;
//using PagedList.Core;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity.Core;
//using System.Linq;
//using Framework.Tools;

//namespace Framework.Services.Site.Pages.Conteudo
//{
//    public class BlogService : IBlogService
//    {
//        private static ApplicationDbContext _context;
//        private static ItemService _srvItem;
//        private static HeadService _srvHead;
//        private static BodyService _srvBody;
//        private static BlogFactory _faBlog;
//        private static CategoriaFactory _faCategoria;
//        private readonly ImagemFactory _faImagem;
//        public BlogService(ApplicationDbContext context)
//        {
//            _context = context;
//            _srvItem = new ItemService(context);
//            _faBlog = new BlogFactory(context);
//            _srvHead = new HeadService();
//            _srvBody = new BodyService();
//            _faCategoria = new CategoriaFactory(context);
//            _faImagem = new ImagemFactory(context);
//        }

//        public BlogPage List(string category, string search, string tags, int? page)
//        {
//            List<Blog> Blogs = new List<Blog>();

//            BlogPage model = new BlogPage();

//            int Pagesize = 10;
//            int PageNumber = page == 0 ? 1 : page != null ? page.ToInt() : 1;

//            try
//            {
//                #region Filter Category
//                if (string.IsNullOrEmpty(category))
//                {
//                    Blogs = _faBlog.GetAllAtivoAndCategoryActive().ToList();
//                }

//                if (!string.IsNullOrEmpty(category))
//                {
//                    Blogs = _faBlog.GetAllAtivoByCategory(category).ToList();
//                }

//                if (category == "GetAll")
//                {
//                    Blogs = _faBlog.GetAllAtivo().ToList();
//                }

//                #endregion

//                #region Search
//                if (!string.IsNullOrEmpty(search))
//                {
//                    Blogs = Blogs.Where(b => b.Titulo != null &&
//                                                   b.Titulo.ToLower().Contains(search)).ToList();
//                }
//                #endregion

//                #region Pagination
//                IQueryable<Item> BlogPosts = Creator(Blogs);
//                model.Posts = BlogPosts.ToPagedList(PageNumber, Pagesize);

//                model.PageSize = Pagesize;
//                model.PageTotal = Blogs.Count();
//                model.PageNumber = model.Posts.PageNumber;
//                model.IsLastPage = model.Posts.IsLastPage;
//                model.IsFirstPage = model.Posts.IsFirstPage;
//                model.HasPreviousPage = model.Posts.HasPreviousPage;
//                model.HasNextPage = model.Posts.HasNextPage;
//                #endregion
//            }
//            catch (EntityException ex)
//            {
//                throw new Exception("500InternalServerError", new Exception());
//            }

//            return model;
//        }

//        public BlogDetailPage Detail(string url)
//        {
//            Blog blog = _faBlog.GetByUrl(url);

//            if (blog != null)
//            {
//                BlogDetailPage model = new BlogDetailPage
//                {
//                    Head = _srvHead.Build(blog),
//                    Body = _srvBody.Build(blog),
//                    Detail = _srvItem.Build(blog),
//                    Imagens = _faImagem.GetImagemsByTableActionAndTableId("Blogs", blog.Id),

//                };

//                return model;
//            }

//            throw new Exception("404NotFound", new Exception());

//        }

//		public List<Blog> GetDestaque()
//		{
//			List<Blog> Blogs = _faBlog.GetDestaque();
//			List<Blog> BlogDestaque = new List<Blog>();

//			foreach (var item in Blogs)
//			{
//				BlogDestaque.Add(CreateMinBlogs(item));
//			}

//			return BlogDestaque;

//		}

//		public List<CategoriaDetailPage> GetAllCategory()
//        {
//            List<Categoria> categoria = _faCategoria.GetAtivos();

//            List<CategoriaDetailPage> categoriasDetalhadas = new List<CategoriaDetailPage>();

//            if (categoria != null)
//            {


//                foreach (Categoria categ in categoria)
//                {
//                    CategoriaDetailPage model = new CategoriaDetailPage();

//                    model.Id = categ.Id;
//                    model.Titulo = categ.Titulo;
//                    model.Url = categ.Url;
//                    model.Blogs = CreateMinBlog(categ);

//                    categoriasDetalhadas.Add(model);
//                }


//            }

//            else
//            {
//                throw new Exception("404NotFound", new Exception());
//            }

//            return categoriasDetalhadas;

//        }

//        public List<Blog> CreateMinBlog(Categoria categ)
//        {
//            List<Blog> blogs = _faBlog.GetBlogsByCategoriaId(categ.Id);

//            List<Blog> blog = new List<Blog>();
//            if (blogs != null)
//            {
//                foreach (var item in blogs)
//                {
//                    blog.Add(CreateMinBlogs(item));
//                }
//                return blog;

//            }
//            throw new Exception("404NotFound", new Exception());
//        }

//        public Blog CreateMinBlogs(Blog blog)
//        {
//            Blog b = new Blog();
//            b.Id = blog.Id;
//            b.Titulo = blog.Titulo;
//            b.Url = blog.Url;
//            b.CategoriaId = blog.CategoriaId;
//            b.CategoriaTitle = blog.Categoria.Titulo;
//            b.Descricao = blog.Descricao;
//            b.Subtitulo = blog.Subtitulo;
//            b.Imagem = blog.Imagem;

//            return b;

//        }

//        //public List<Categoria> GetAllCategory()
//        //{
//        //    List<Categoria> categories = _faCategoria.GetAllAtivoHavingBlogs();
//        //    List<Categoria> model = new List<Categoria>();

//        //    if(categories != null)
//        //    {
//        //        foreach (var item in categories)
//        //        {
//        //           model.Add(CreateCategoriaBlog(item));
//        //        }
//        //        return model.ToList();
//        //    }

//        //    throw new Exception("404NotFound", new Exception());

//        //}

//        //private Categoria CreateCategoriaBlog(Categoria categoria)
//        //{
//        //    List<Blog> Blogs = _faBlog.GetBlogsByCategoriaId(categoria.Id).ToList();
//        //    List<Blog> blog = new List<Blog>();

//        //    foreach (var item in Blogs)
//        //    {
//        //        blog.Add(item);
//        //    }

//        //    return new Categoria()
//        //    {
//        //        Id = categoria.Id,
//        //        Titulo = categoria.Titulo,
//        //        Url = categoria.Url,
//        //        Blogs = blog
//        //    };
//        //}

//        private IQueryable<Item> Creator(List<Blog> blogs)
//        {
//            List<Item> List = new();

//            foreach (var item in blogs)
//            {
//                List.Add(_srvItem.Build(item));
//            }

//            return List.AsQueryable();
//        }

//    }
//}
