using Data;
using Microsoft.AspNetCore.Mvc;
using Painel.Models.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panel.Components.ViewComponents.Propriedade
{
    public class CategoriaViewComponent : ViewComponent
    {
        private static ApplicationDbContext _context;
        public CategoriaViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            int ID = id.ToInt();

            List<DropItem> dropItens = new List<DropItem>();

            if (ID > 0)
                _context.Categoria.Where(b => b.Ativo == true && b.Id == ID).ToList().ForEach(obj =>
                {
                    dropItens.Add(new DropItem(obj.Titulo, obj.Id.ToString()));
                });
            else
                _context.Categoria.Where(b => b.Ativo == true).ToList().ForEach(obj =>
                {
                    dropItens.Add(new DropItem(obj.Titulo, obj.Id.ToString()));
                });

            return View("~/Views/Components/_SelectListItem.cshtml", dropItens.ToList());
        }
    }


}
