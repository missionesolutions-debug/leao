using Data;
using Microsoft.AspNetCore.Mvc;
using Painel.Models.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Panel.Components.ViewComponents
{
    public class UsuarioViewComponent : ViewComponent
    {
        private static ApplicationDbContext _context;
        public UsuarioViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            int ID = id.ToInt();

            List<DropItem> dropItens = new List<DropItem>();

            if (ID > 0)
                _context.Usuario.Where(b => b.Ativo == true && b.Id == ID).ToList().ForEach(obj =>
                {
                    dropItens.Add(new DropItem(obj.Nome, obj.Id.ToString()));
                });
            else
                _context.Usuario.Where(b => b.Ativo == true).ToList().ForEach(obj =>
                {
                    dropItens.Add(new DropItem(obj.Nome, obj.Id.ToString()));
                });

            return View("~/Views/Components/_SelectListItem.cshtml", dropItens.ToList());
        }
    }


}
