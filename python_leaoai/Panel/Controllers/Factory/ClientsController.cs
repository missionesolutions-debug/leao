using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Data;
using Data.Repositories;
using Data.Models;
using Panel.Models.Pages;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Framework.Data.Models.Factory;
using Painel.Models.Pages;
using Painel.Models;
using Panel.Builders;
using Panel.Models.Pages.Factory;

namespace Panel.Controllers
{
    [Authorize(Roles = "Admin,PowerUser")]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientsController(ApplicationDbContext context, ClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Clients
        public IActionResult Index()
        {
            var page = _context.Page.FirstOrDefault(b => b.Route == "/Clients");
            var clients = _clientFactory.GetList(); // Retorna os registros não excluídos
            var viewModel = new ClientPage
            {
                Page = page,
                ContentConfig = ContentBuilder.GenerateContentConfig(page),
                Clients = clients
            };

            return View("~/Views/Clients/Index.cshtml", viewModel);
        }

        // GET: /Clients/Create/{id?}
        public IActionResult Create(int id = 0)
        {
            var page = _context.Page.FirstOrDefault(b => b.Route == "/Clients");
            Client client;
            if (id > 0)
            {
                client = _clientFactory.GetObj(id);
            }
            else
            {
                client = new Client();
            }

            var viewModel = new ClientPage
            {
                Page = page,
                ContentConfig = ContentBuilder.GenerateContentConfig(page),
                Client = client
            };

            return View("~/Views/Clients/Create.cshtml", viewModel);
        }

        // POST: /Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Client client)
        {
            var page = _context.Page.FirstOrDefault(b => b.Route == "/Clients");
            try
            {

                client.CEP = "";
                client.CNPJ = "";
                client.Endereco = "";
                client.Telefone = "";

                if (client.Id > 0)
                {
                    _clientFactory.UpdateObj(client);
                    return Redirect("/Clients?result=EditadoComSucesso");
                }
                else
                {
                    client.DataCriacao = DateTime.UtcNow;
                    client.Ativo = true;
                    client.Excluido = false;
                    _clientFactory.SaveObj(client);
                    return Redirect("/Clients?result=CriadoComSucesso");
                }
            }
            catch (Exception ex)
            {
                var viewModel = new ClientPage
                {
                    Page = page,
                    ContentConfig = ContentBuilder.GenerateContentConfig(page),
                    Client = client,
                    Info = new Info { Mensagem = "Erro ao criar objeto: " + ex.Message }
                };
                return View("~/Views/Clients/Create.cshtml", viewModel);
            }
        }

        // POST: /Clients/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _clientFactory.DeleteObj(id);
            return Redirect("/Clients?result=DeletadoComSucesso");
        }
    }
}
