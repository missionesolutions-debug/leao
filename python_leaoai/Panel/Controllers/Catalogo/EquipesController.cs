using Data;
using Data.Models.Catalogo;
using Framework.Factories.Catalogo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders;
using Painel.Builders.Conteudo;
using Painel.Models;

namespace Painel.Controllers.Catalogo
{
    [Authorize]
    public class EquipesController : Controller
    {
        private static EquipeBuilder _builder;
        private static EquipeFactory _factory;
        private static PagesBuilder _pgBuilder;

        public EquipesController(ApplicationDbContext context)
        {
            _factory = new EquipeFactory(context);
            _builder = new EquipeBuilder(context);
            _pgBuilder = new PagesBuilder(context);
        }

        /// <summary>
        /// Listar os registros
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View("~/Views/_Catalogo/Equipes/Index.cshtml", _pgBuilder.BuildViewEquipes(new Info(), _builder.List()));
        }

        /// <summary>
        /// Detalhe do registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            if (id > 0)
                return View("~/Views/_Catalogo/Equipes/Detail.cshtml", _pgBuilder.BuildViewEquipe(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Catalogo/Equipes/Detail.cshtml", _pgBuilder.BuildViewEquipe(new Info(), new Equipe()));
        }


        /// <summary>
        /// Get do create
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Create(int id)
        {
            if (id > 0)
                return View("~/Views/_Catalogo/Equipes/Create.cshtml", _pgBuilder.BuildViewEquipe(new Info(), _factory.GetObj(id)));
            else
                return View("~/Views/_Catalogo/Equipes/Create.cshtml", _pgBuilder.BuildViewEquipe(new Info(), new Equipe()));
        }

        /// <summary>
        /// Post para criar
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(Equipe obj)
        {
            try
            {
                if (_builder.SaveOrUpdate(obj).DataEdicao != null)
                {
                    return Redirect("/Equipes?result=EditadoComSucesso");
                }
                else
                {
                    return Redirect("/Equipes?result=CriadoComSucesso");
                }
            }
            catch (Exception)
            {
                return View(_pgBuilder.BuildViewEquipe(new Info() { Mensagem = "Erro ao criar objeto" }, obj));
            }

        }

        /// <summary>
        /// Rota para deletar o Objeto
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            _builder.DeleteObj(Id);

            return Redirect("/Equipes?result=DeletadoComSucesso");
        }

    }
}
