using Data.Models.Conteudo;
using Data.Models.PainelConfig;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class PaginaPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<Pagina> Paginas { get; set; }
        public Pagina Pagina { get; set; }
    }
}
