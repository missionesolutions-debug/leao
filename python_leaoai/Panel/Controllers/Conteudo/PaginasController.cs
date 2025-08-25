using Data;
using Data.Models.Conteudo;
using Framework.Factories.Conteudo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders;
using Painel.Builders.Conteudo;
using Painel.Models;

namespace Painel.Controllers.Conteudo
{
    [Authorize]
    public class PaginasController : Controller
    {
        private static PaginaBuilder _builder;
        private static PaginaFactory _factory;
        private static PagesBuilder _pgBuilder;

        public PaginasController(ApplicationDbContext context)
        {
            _factory = new PaginaFactory(context);
            _builder = new PaginaBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        public IActionResult Index()
        {

           
            
            return View("~/Views/_Conteudo/Paginas/Index.cshtml", _pgBuilder.BuildViewPaginas(new Info(), _builder.List()));
        }

        public IActionResult Detail(int id)
        {
            if (id > 0)
                return View("~/Views/_Conteudo/Paginas/Detail.cshtml", _pgBuilder.BuildViewPagina(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Conteudo/Paginas/Detail.cshtml", _pgBuilder.BuildViewPagina(new Info(), new Pagina()));
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Conteudo/Paginas/Create.cshtml", _pgBuilder.BuildViewPagina(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Conteudo/Paginas/Create.cshtml", _pgBuilder.BuildViewPagina(new Info(), new Pagina()));
        }

        [HttpPost]
        public IActionResult Create(Pagina obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/Paginas?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/Paginas?result=CriadoComSucesso");
                }
            }
            catch (System.Exception ex)
            {
                return View(_pgBuilder.BuildViewPagina(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
            }

        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _builder.DeleteObj(Id);

            return Redirect("/Paginas?result=DeletadoComSucesso");
        }

    }
}
