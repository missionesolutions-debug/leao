using Data;
using Data.Models.Pessoa;
using Framework.Factories.Pessoa;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Painel.Builders.Pessoa;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Painel.Controllers
{
    public class LoginController : Controller
    {

        private static ApplicationDbContext _context;
        private static UsuarioBuilder _usuarioBuilder;
        private static UsuarioFactory _usuarioFactory;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
            _usuarioFactory = new UsuarioFactory(context);
            _usuarioBuilder = new UsuarioBuilder(context);
        }

        public IActionResult Authentication()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authentication(Usuario _usuario, string ReturnUrl)
        {
            if (_usuarioFactory.GetAllAtivo().Any(u => u.Login == _usuario.Login && u.Password == _usuario.Password))
            {
                Usuario user = _usuarioFactory.GetAllAtivo().Where(u => u.Login == _usuario.Login && u.Password == _usuario.Password).FirstOrDefault();

                var userClaims = new List<Claim>()
                {
                    //define o cookie
                    new Claim(ClaimTypes.Name, _usuario.Login),
                    new Claim(ClaimTypes.Email,user.Email ),
                    
                };

                if(user.RoleGate != null)
                {
                    List<String> Roles = user.RoleGate.Split(",").ToList();

                    foreach (var item in Roles)
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, item));
                    }
                }

                var minhaIdentity = new ClaimsIdentity(userClaims, "LoginAuthPanel");
                var userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });
                //cria o cookie
                HttpContext.SignInAsync(userPrincipal);
                if(ReturnUrl != null && ReturnUrl.Length > 0)
                    return Redirect(ReturnUrl);
                return Redirect("~/");
            }

            ViewBag.Message = "Credenciais inválidas...";

            return View(_usuario);

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Authentication", "Login");
        }
    }
}
