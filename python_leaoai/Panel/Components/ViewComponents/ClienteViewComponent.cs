using Data;
using Microsoft.AspNetCore.Mvc;
using Painel.Models.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Components.ViewComponents
{
    public class ClienteViewComponent : ViewComponent
    {
        private static ApplicationDbContext _context;
        public ClienteViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            int ID = id.ToInt();

            List<DropItem> dropItens = new List<DropItem>();

            _context.Cliente.Where(b => b.Ativo == true).ToList().ForEach(obj =>
            {
                dropItens.Add(new DropItem(obj.Nome, obj.Id.ToString()));
            });

            return View("~/Views/Components/_SelectListItem.cshtml", dropItens.OrderBy(b => b.Name).ToList());
        }
    }


}
