using Data.Models.PainelConfig;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class MenusPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<Menu> Menus { get; set; }
        public Menu Menu { get; set; }
    }
}
