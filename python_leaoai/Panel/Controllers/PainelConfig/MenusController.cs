using Data;
using Data.Models.PainelConfig;
using Framework.Factories.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders.PainelConfig;

namespace Painel.Controllers.PainelConfig
{
    [Authorize]
    public class MenusController : Controller
    {
        private static MenuBuilder _Builder;
        private static MenuFactory _Factory;

        public MenusController(ApplicationDbContext context)
        {
            _Factory = new MenuFactory(context);
            _Builder = new MenuBuilder(context);
        }

        public IActionResult Index()
        {

            return View("~/Views/_PainelConfig/Menus/Index.cshtml", _Builder.List());
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_PainelConfig/Menus/Create.cshtml", _Factory.GetObj(id));
            else
                return View("~/Views/_PainelConfig/Menus/Create.cshtml", new Menu());
        }

        [HttpPost]
        public IActionResult Create(Menu obj)
        {
            if (_Builder.SaveOrUpdate(obj).DataEdicao != null)
            {
                return Redirect("/Menus?result=EditadoComSucesso");
            }
            else
            {
                return Redirect("/Menus?result=CriadoComSucesso");
            }
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _Builder.DeleteObj(Id);

            return Redirect("/Menus?result=DeletadoComSucesso");
        }

    }
}
