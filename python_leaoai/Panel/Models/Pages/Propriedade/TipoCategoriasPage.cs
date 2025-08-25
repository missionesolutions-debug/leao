using Data.Models.PainelConfig;
using Data.Models.Propriedade;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class TipoCategoriasPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<TipoCategoria> TipoCategorias { get; set; }
        public TipoCategoria TipoCategoria { get; set; }
    }
}
