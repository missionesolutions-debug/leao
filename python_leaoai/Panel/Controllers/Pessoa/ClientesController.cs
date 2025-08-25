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
    public class ClientesController : Controller
    {
        private static ClienteBuilder _builder;
        private static ClienteFactory _factory;
        private static PagesBuilder _pgBuilder;

        public ClientesController(ApplicationDbContext context)
        {
            _factory = new ClienteFactory(context);
            _builder = new ClienteBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        public IActionResult Index()
        {
            return View("~/Views/_Pessoa/Clientes/Index.cshtml", _pgBuilder.BuildViewClientes(new Info(), _builder.List()));
        }


        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Pessoa/Clientes/Create.cshtml", _pgBuilder.BuildViewCliente(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Pessoa/Clientes/Create.cshtml", _pgBuilder.BuildViewCliente(new Info(), new Cliente()));
        }

        [HttpPost]
        public IActionResult Create(Cliente obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/clientes?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/clientes?result=CriadoComSucesso");
                }
            }
            catch (System.Exception ex)
            {
                return View(_pgBuilder.BuildViewCliente(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
            }
         
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _builder.DeleteObj(Id);

            return Redirect("/clientes?result=DeletadoComSucesso");
        }
   
    }
}
