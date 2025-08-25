using Data;
using Data.Models.Pessoa;
using Framework.Factories.Pessoa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders;
using Painel.Builders.Pessoa;
using Painel.Models;

namespace Painel.Controllers.Pessoa
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private static UsuarioBuilder _builder;
        private static UsuarioFactory _factory;
        private static PagesBuilder _pgBuilder;

        public UsuariosController(ApplicationDbContext context)
        {
            _factory = new UsuarioFactory(context);
            _builder = new UsuarioBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        public IActionResult Index()
        {
            return View("~/Views/_Pessoa/Usuarios/Index.cshtml", _pgBuilder.BuildViewUsuarios(new Info(), _builder.List()));
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Pessoa/Usuarios/Create.cshtml", _pgBuilder.BuildViewUsuario(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Pessoa/Usuarios/Create.cshtml", _pgBuilder.BuildViewUsuario(new Info(), new Usuario()));
        }

        [HttpPost]
        public IActionResult Create(Usuario obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/usuarios?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/usuarios?result=CriadoComSucesso");
                }
            }
            catch (System.Exception ex)
            {
                return View(_pgBuilder.BuildViewUsuario(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
            }

        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _builder.DeleteObj(Id);

            return Redirect("/usuarios?result=DeletadoComSucesso");
        }

    }
}
