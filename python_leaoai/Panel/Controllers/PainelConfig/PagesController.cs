using Data;
using Data.Models.PainelConfig;
using Framework.Factories.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders.PainelConfig;

namespace Painel.Controllers.PainelConfig
{
    [Authorize]
    public class PagesController : Controller
    {
        private static PageBuilder _Builder;
        private static PageFactory _Factory;

        public PagesController(ApplicationDbContext context)
        {
            _Factory = new PageFactory(context);
            _Builder = new PageBuilder(context);
        }

        public IActionResult Index()
        {

            return View("~/Views/_PainelConfig/Pages/Index.cshtml", _Builder.List());
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_PainelConfig/Pages/Create.cshtml", _Factory.GetObj(id));
            else
                return View("~/Views/_PainelConfig/Pages/Create.cshtml", new Page());
        }

        [HttpPost]
        public IActionResult Create(Page obj)
        {
            if (_Builder.SaveOrUpdate(obj).DataEdicao != null)
            {
                return Redirect("/Pages?result=EditadoComSucesso");
            }
            else
            {
                return Redirect("/Pages?result=CriadoComSucesso");
            }
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _Builder.DeleteObj(Id);

            return Redirect("/Pages?result=DeletadoComSucesso");
        }

    }
}
