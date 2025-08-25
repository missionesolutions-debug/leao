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
    public class SubCategoriasController : Controller
    {
        private static SubCategoriaBuilder _builder;
        private static SubCategoriaFactory _factory;
        private static PagesBuilder _pgBuilder;

        public SubCategoriasController(ApplicationDbContext context)
        {
            _factory = new SubCategoriaFactory(context);
            _builder = new SubCategoriaBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        public IActionResult Index()
        {
            return View("~/Views/_Propriedade/SubCategorias/Index.cshtml", _pgBuilder.BuildViewSubCategorias(new Info(), _builder.List()));
        }

        public IActionResult Detail(int id)
        {
            if (id > 0)
                return View("~/Views/_Propriedade/SubCategorias/Detail.cshtml", _pgBuilder.BuildViewSubCategoria(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Propriedade/SubCategorias/Detail.cshtml", _pgBuilder.BuildViewSubCategoria(new Info(), new SubCategoria()));
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Propriedade/SubCategorias/Create.cshtml", _pgBuilder.BuildViewSubCategoria(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Propriedade/SubCategorias/Create.cshtml", _pgBuilder.BuildViewSubCategoria(new Info(), new SubCategoria()));
        }

        [HttpPost]
        public IActionResult Create(SubCategoria obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/subcategorias?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/subcategorias?result=CriadoComSucesso");
                }
            }
            catch (System.Exception ex)
            {
                return View(_pgBuilder.BuildViewSubCategoria(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
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
