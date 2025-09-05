using Data.Models.PainelConfig;
using Data.Models.Propriedade;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class SubCategoriasPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<SubCategoria> SubCategorias { get; set; }
        public SubCategoria SubCategoria { get; set; }
    
    }
}
