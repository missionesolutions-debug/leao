using Data;
using Data.Models.Conteudo;
using Framework.Domain.Core;
using Framework.Domain.Dtos.Content;
using Framework.Domain.Site.Component;
using Framework.Factories.Conteudo;
using Framework.Factories.Core;
using Framework.Factories.Propriedade;
using Framework.Repositories;
using Framework.Services.ApplicationServices.Core;
using Framework.Services.ApplicationServices.Pages;
using PagedList.Core;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Services.ApplicationServices.Content
{
    public class BlogService : GenericService<Blog, IRepository<Blog>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemService _srvItem;
        private readonly IRepository<Blog> _repository;
        private readonly BlogFactory _faBlog;
        private readonly HeadContentService _srvHead;
        private readonly BodyContentService _srvBody;
        private readonly ImagemFactory _faImagem;
        private readonly CategoriaFactory _faCategoria;

        public BlogService(ApplicationDbContext context, ItemService itemService, IRepository<Blog> repository, BlogFactory blogFactory, HeadContentService headService, BodyContentService bodyService, ImagemFactory imagemFactory, CategoriaFactory categoriaFactory) : base(context, itemService, repository)
        {
            _context = context;
            _srvItem = itemService;
            _repository = repository;
            _faBlog = blogFactory;
            _srvHead = headService;
            _srvBody = bodyService;
            _faImagem = imagemFactory;
            _faCategoria = categoriaFactory;
        }

        public BlogPage List(string category, string search, string tags, int? page)
        {
            if (!search.IsNullOrEmpty())
                search = search.ToLower();

            #region Initialize
            List<Blog> Blogs = new List<Blog>();

            BlogPage blogDTO = new BlogPage();

            int Pagesize = 12;
            int PageNumber = page == 0 ? 1 : page != null ? page.ToInt() : 1;
            #endregion

            #region Filter Category
            if (string.IsNullOrEmpty(category))
                Blogs = _faBlog.GetAllAtivoAndCategoryActive().ToList();

            if (!string.IsNullOrEmpty(category))
                Blogs = _faBlog.GetAllAtivoByCategory(category).ToList();

            if (category == "GetAll")
                Blogs = _faBlog.GetAllAtivo().ToList();
            #endregion

            #region Search
            if (!string.IsNullOrEmpty(search))
                Blogs = Blogs.Where(b => b.Titulo != null &&
                                               b.Titulo.ToLower().Contains(search) || b.Descricao.ToLower().Contains(search)).ToList();
            #endregion

            var allTags = Blogs
                           .Where(blog => blog.Tags != null) // Verifica se o campo Tags não é nulo
                           .SelectMany(blog => blog.Tags.Split(';')) // Divide as tags separadas por ';'
                           .Distinct() // Pega apenas tags únicas
                           .ToArray(); // Converte para array

            // Junta todas as tags em uma string única separada por ponto e vírgula
            blogDTO.Page.PageTitle = "Blog";
            blogDTO.Page.Tags = string.Join(";", allTags);

            var categorias = _faCategoria.GetAllAtivoHavingBlogs();

            foreach (var item in categorias)
            {
                item.Blogs = null;

                blogDTO.Categorias.Add(_srvItem.Build(item));
            }

            var posts = _faBlog.GetDestaque();

            foreach (var item in posts)
            {
                item.Descricao = "";
                item.Categoria = null;

                blogDTO.Destaques.Add(_srvItem.Build(item));
            }

            Blogs = Blogs.OrderByDescending(b => b.Id).ToList();

            #region Pagination
            IQueryable<Item> BlogPosts = Creator(Blogs);
            blogDTO.Posts = BlogPosts.ToPagedList(PageNumber, Pagesize);

            Pagination pagination = new Pagination
            {
                PageSize = Pagesize,
                PageTotal = Blogs.Count(),
                PageNumber = blogDTO.Posts.PageNumber,
                IsLastPage = blogDTO.Posts.IsLastPage,
                IsFirstPage = blogDTO.Posts.IsFirstPage,
                HasPreviousPage = blogDTO.Posts.HasPreviousPage,
                HasNextPage = blogDTO.Posts.HasNextPage,
                StartPage = 1,
                totalPages = blogDTO.Posts.PageCount,
                totalItems = blogDTO.Posts.Count,
                EndPage = blogDTO.Posts.PageCount,
                pages = Enumerable.Range(1, blogDTO.Posts.PageCount).ToArray()
            };

            blogDTO.Pagination = pagination;
            #endregion

            return blogDTO;
        }



        public BlogPage ListDestaque(int? page)
        {
            #region Initialize
            List<Blog> Blogs = new List<Blog>();

            BlogPage blogDTO = new BlogPage();

            int Pagesize = 12;
            int PageNumber = page == 0 ? 1 : page != null ? page.ToInt() : 1;
            #endregion

            Blogs = _faBlog.GetDestaque();

            blogDTO.Page.PageTitle = "Blog";

            var allTags = Blogs
                    .Where(blog => blog.Tags != null) // Verifica se o campo Tags não é nulo
                    .SelectMany(blog => blog.Tags.Split(';')) // Divide as tags separadas por ';'
                    .Distinct() // Pega apenas tags únicas
                    .ToArray(); // Converte para array

            // Junta todas as tags em uma string única separada por ponto e vírgula
            blogDTO.Page.Tags = string.Join(";", allTags);

            #region Pagination
            IQueryable<Item> BlogPosts = Creator(Blogs);

            blogDTO.Posts = BlogPosts.ToPagedList(PageNumber, Pagesize);

            Blogs = Blogs.OrderByDescending(b => b.Id).ToList();

            Pagination pagination = new Pagination
            {
                PageSize = Pagesize,
                PageTotal = Blogs.Count(),
                PageNumber = blogDTO.Posts.PageNumber,
                IsLastPage = blogDTO.Posts.IsLastPage,
                IsFirstPage = blogDTO.Posts.IsFirstPage,
                HasPreviousPage = blogDTO.Posts.HasPreviousPage,
                HasNextPage = blogDTO.Posts.HasNextPage,
                StartPage = 1,
                totalPages = blogDTO.Posts.PageCount,
                totalItems = blogDTO.Posts.Count,
                EndPage = blogDTO.Posts.PageCount,
                pages = Enumerable.Range(1, blogDTO.Posts.PageCount).ToArray()
            };

            blogDTO.Pagination = pagination;
            #endregion


            return blogDTO;

        }

        public BlogPageDetail Detail(string url)
        {
            Blog obj = _faBlog.GetByUrlOnlyPost(url);

            if (obj != null)
            {

                BlogPageDetail model = new()
                {
                    Page = _srvItem.Build(obj),
                };

                if (model.Page is not null)
                {

                    var categorias = _faCategoria.GetAllAtivoHavingBlogs();

                    foreach (var item in categorias)
                    {
                        item.Blogs = null;

                        model.Categorias.Add(_srvItem.Build(item));
                    }

                    var posts = _faBlog.GetBlogsByCategoriaId(obj.CategoriaId);

                    foreach (var item in posts.Where(b=> b.Id != obj.Id))
                    {
                        item.Descricao = "";
                        item.Categoria = null;

                        model.Posts.Add(_srvItem.Build(item));
                    }

                    try
                    {
                        var imagens = _faImagem.GetImagemsByTableActionAndTableId("Blogs", obj.Id);


                        foreach (var item in imagens)
                        {
                            model.Page.imagens.Add(_srvItem.Build(item));
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
                return model;
            }

            throw new Exception("404NotFound", new Exception(""));

        }


        public List<Item> ListCategorias()
        {
            var categorias = _faCategoria.GetAllAtivoHavingBlogs();

            var itens = new List<Item>();

            foreach (var categoria in categorias)
            {
                var item = _srvItem.Build(categoria);
                itens.Add(item);
            }

            return itens;
        }

        private IQueryable<Item> Creator(List<Blog> blogs)
        {
            List<Item> List = [];

            foreach (var item in blogs)
            {
                List.Add(_srvItem.Build(item));
            }

            return List.AsQueryable();
        }
    }
}
