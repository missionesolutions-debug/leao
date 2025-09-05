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
    public class TipoCategoriasController : Controller
    {
        private static TipoCategoriaBuilder _builder;
        private static TipoCategoriaFactory _factory;
        private static PagesBuilder _pgBuilder;

        public TipoCategoriasController(ApplicationDbContext context)
        {
            _factory = new TipoCategoriaFactory(context);
            _builder = new TipoCategoriaBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        public IActionResult Index()
        {
            return View("~/Views/_Propriedade/TipoCategorias/Index.cshtml", _pgBuilder.BuildViewTipoCategorias(new Info(), _builder.List()));
        }

        public IActionResult Detail(int id)
        {
            if (id > 0)
                return View("~/Views/_Propriedade/TipoCategorias/Detail.cshtml", _pgBuilder.BuildViewTipoCategoria(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Propriedade/TipoCategorias/Detail.cshtml", _pgBuilder.BuildViewTipoCategoria(new Info(), new TipoCategoria()));
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Propriedade/TipoCategorias/Create.cshtml", _pgBuilder.BuildViewTipoCategoria(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Propriedade/TipoCategorias/Create.cshtml", _pgBuilder.BuildViewTipoCategoria(new Info(), new TipoCategoria()));
        }

        [HttpPost]
        public IActionResult Create(TipoCategoria obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/tipocategorias?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/tipocategorias?result=CriadoComSucesso");
                }
            }
            catch (System.Exception)
            {
                return View(_pgBuilder.BuildViewTipoCategoria(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
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
