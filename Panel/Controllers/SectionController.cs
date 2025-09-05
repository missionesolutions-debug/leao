using Microsoft.AspNetCore.Mvc;

namespace Panel.Controllers
{
    public class SectionController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
