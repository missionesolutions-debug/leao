using Data;
using Framework.Factories.Pessoa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders;
using System.Threading.Tasks;

namespace Painel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static DashboardBuilder _builder;
        private static UsuarioFactory _faUsuario;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _faUsuario = new UsuarioFactory(context);
            _builder = new DashboardBuilder(context, _faUsuario.GetObjByLogin(GetLoginUserName()));
        }

        public async Task<ActionResult> Index()
        {
            return View("~/Views/Home/Index.cshtml",await _builder.Build());
        }

        #region UserThings
        public string GetLoginUserName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
        #endregion


    }
}
