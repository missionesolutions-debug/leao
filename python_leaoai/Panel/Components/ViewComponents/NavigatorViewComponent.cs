using Data;
using Data.Models.PainelConfig;
using Framework.Factories.System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Components.ViewComponents
{
    public class NavigatorViewComponent : ViewComponent
    {
        private static ApplicationDbContext _context;
        private static MenuFactory _Factory;
        public NavigatorViewComponent(ApplicationDbContext context)
        {
            _context = context;
            _Factory = new MenuFactory(context);
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Menu> menu = _Factory.GetAllAtivoWithPages().ToList();

            /*_context.Menu.Where(b=> b.Ativo == true).Include(a => a.Page).OrderByDescending(b=> b.Ordem).ToList();*/

            List<Menu> menuFinal = new List<Menu>();

            bool skip = false;

            foreach (var item in menu)
            {
                if (item.RoleGate != null)
                {
                    List<string> Roles = item.RoleGate.Split(",").ToList();

                    foreach (var obj in Roles)
                    {
                        if (skip == true)
                        {

                        }
                        else
                        {
                            if (User.IsInRole(obj))
                            {
                                menuFinal.Add(item);
                                skip = true;
                            }
                        }

                    }

                    skip = false;
                }
            }

            return View("~/Views/Shared/SystemViews/_Menu.cshtml", menuFinal.ToList());
        }
    }


}
