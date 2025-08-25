using Data;
using Data.Models.Pessoa;
using Microsoft.AspNetCore.Mvc;

namespace Painel.Controllers
{
    public class BaseController : Controller
    {
        public Usuario GetUsuario(ApplicationDbContext _context, String Identity)
        {
            return new Usuario();
        }
    }
}
