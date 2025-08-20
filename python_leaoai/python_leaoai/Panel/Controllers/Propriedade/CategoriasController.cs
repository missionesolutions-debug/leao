using Data;
using Data.Models.Propriedade;
using Framework.Factories.Propriedade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders;
using Painel.Builders.Propriedade;
using Painel.Models;

namespace Painel.Controllers.Propriedade
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private static CategoriaBuilder _builder;
        private static CategoriaFactory _factory;
        private static PagesBuilder _pgBuilder;

        public CategoriasController(ApplicationDbContext context)
        {
            _factory = new CategoriaFactory(context);
            _builder = new CategoriaBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        public IActionResult Index()
        {
            return View("~/Views/_Propriedade/Categorias/Index.cshtml", _pgBuilder.BuildViewCategorias(new Info(), _builder.List()));
        }

        public IActionResult Detail(int id)
        {
            if (id > 0)
                return View("~/Views/_Propriedade/Categorias/Detail.cshtml", _pgBuilder.BuildViewCategoria(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Propriedade/Categorias/Detail.cshtml", _pgBuilder.BuildViewCategoria(new Info(), new Categoria()));
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Propriedade/Categorias/Create.cshtml", _pgBuilder.BuildViewCategoria(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Propriedade/Categorias/Create.cshtml", _pgBuilder.BuildViewCategoria(new Info(), new Categoria()));
        }

        [HttpPost]
        public IActionResult Create(Categoria obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/categorias?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/categorias?result=CriadoComSucesso");
                }
            }
            catch (System.Exception ex)
            {
                return View(_pgBuilder.BuildViewCategoria(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
            }

        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _builder.DeleteObj(Id);

            return Redirect("/tipocategorias?result=DeletadoComSucesso");
        }

    }
}
