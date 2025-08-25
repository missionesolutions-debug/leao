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
using Painel.Models;
using Panel.Builders;
using Panel.Models.Pages.Factory;
using Data.Models.Pessoa;

namespace Panel.Controllers
{
    [Authorize(Roles = "Admin,PowerUser")]
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SupplierFactory _supplierFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SuppliersController(ApplicationDbContext context, SupplierFactory supplierFactory, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _supplierFactory = supplierFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Suppliers
        public IActionResult Index()
        {
            var page = _context.Page.FirstOrDefault(b => b.Route == "/Suppliers");
            var suppliers = _supplierFactory.GetList();
            var viewModel = new SupplierPage
            {
                Page = page,
                ContentConfig = ContentBuilder.GenerateContentConfig(page),
                Suppliers = suppliers
            };

            return View("~/Views/Suppliers/Index.cshtml", viewModel);
        }

        // GET: /Suppliers/Create/{id?}
        public IActionResult Create(int id = 0)
        {
            var page = _context.Page.FirstOrDefault(b => b.Route == "/Suppliers");
            Supplier supplier;
            if (id > 0)
            {
                supplier = _supplierFactory.GetObj(id);
            }
            else
            {
                supplier = new Supplier();
            }

            var viewModel = new SupplierPage
            {
                Page = page,
                ContentConfig = ContentBuilder.GenerateContentConfig(page),
                Supplier = supplier
            };

            return View("~/Views/Suppliers/Create.cshtml", viewModel);
        }

        // POST: /Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            var page = _context.Page.FirstOrDefault(b => b.Route == "/Suppliers");
            try
            {
                supplier.CEP = "";
                supplier.CNPJ = "";
                supplier.Endereco = "";
                supplier.Telefone = "";

                if (supplier.Id > 0)
                {
                    _supplierFactory.UpdateObj(supplier);
                    return Redirect("/Suppliers?result=EditadoComSucesso");
                }
                else
                {
                    supplier.DataCriacao = DateTime.UtcNow;
                    supplier.Ativo = true;
                    supplier.Excluido = false;
                    _supplierFactory.SaveObj(supplier);
                    return Redirect("/Suppliers?result=CriadoComSucesso");
                }
            }
            catch (Exception ex)
            {
                var viewModel = new SupplierPage
                {
                    Page = page,
                    ContentConfig = ContentBuilder.GenerateContentConfig(page),
                    Supplier = supplier,
                    Info = new Info { Mensagem = "Erro ao criar objeto: " + ex.Message }
                };
                return View("~/Views/Suppliers/Create.cshtml", viewModel);
            }
        }

        // POST: /Suppliers/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _supplierFactory.DeleteObj(id);
            return Redirect("/Suppliers?result=DeletadoComSucesso");
        }
    }
}
